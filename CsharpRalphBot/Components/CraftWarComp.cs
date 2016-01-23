using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;
using CsharpRalphBot.Database;


namespace CsharpRalphBot.Components
{
    class CraftWarComp : Component
    {
        private string[] commands = {"#attack","#stats","#commands","#upgrade","#buildBarracks","#buildUnits","#add","#maxupgrade","#maxunits"};
        private RDatabase _database;
        private CraftWarCompThread craftWarThread;
        private Random _randomObject;

        public CraftWarComp()
        {
            _database = new RDatabase();
            craftWarThread = new CraftWarCompThread(_database);
            craftWarThread.start();
            _randomObject = new Random();
        }

        public override void ThreadTask()
        {
            throw new NotImplementedException();
        }

        public override bool Check(IrcMessageData msg)
        {
            bool res = false;
            string com = msg.MessageArray[0];
            for(int i = 0; i < commands.Length; i++)
            {
                if (com == commands[i]) res = true;
            }
            return res;
        }

        public override string Handle(IrcMessageData msg)
        {
            string res;
            string com = msg.MessageArray[0];
            switch (com)
            {
                case "#attack":
                    res = Attack(msg);
                    break;
                case "#stats":
                    res = Stats(msg);
                    break;
                case "#commands":
                    res = GetCommands();
                    break;
                case "#upgrade":
                    res = UpgradeMine(msg);
                    break;
                case "#buildBarracks":
                    res = BuildBarracks(msg);
                    break;
                case "#buildUnits":
                    res = BuildUnits(msg);
                    break;
                case "#add":
                    res = AddPlayer(msg);
                    break;
                case "#maxupgrade":
                    res = Maxupgrade(msg);
                    break;
                case "#maxunits":
                    res = MaxUnits(msg);
                    break;
                default:
                    res = null;
                    break;
            }
            return res;
        }

        private string AddPlayer(IrcMessageData msg)
        {
            string res;
            string sender = RealName(msg.From).ToLower();
            if (sender == "voodoohood")
            {
                if(msg.MessageArray.Length >= 2)
                {
                    string userToAdd = msg.MessageArray[1].ToLower();
                    bool playerAlreadyRegistered = _database.isUserRegistered(userToAdd);
                    if (!playerAlreadyRegistered)
                    {
                        _database.addPlayerToCraftWar(userToAdd);
                        res = "User " + userToAdd + " added to the registered players .";
                        DumberLogger.Log(" CraftWarComp: Added user" + userToAdd + " to database, is registered " + _database.isUserRegistered(userToAdd));
                    }
                    else
                    {
                        res = "User " + userToAdd + " is already registered";
                    }
                      
                }
                else
                {
                    res = null;
                    DumberLogger.Log(" CraftWarComp: Format of the message wasnt correct");
                }
            }
            else
            {
                res = null;
                DumberLogger.Log(" CraftWarComp: User other than voodoohood tryed to add a user");
            }
            return res;
        }

        private string GetCommands()
        {
            string res="";
            for(int i = 0; i < commands.Length; i++)
            {
                res = res +" | "+ commands[i];
            }
            return res;
        }


        private string Attack(IrcMessageData msg)
        {
            String res = null;
            if (msg.MessageArray.Length >= 2)
            {
                string attacker = RealName(msg.From).ToLower();
                string defender = RealName(msg.MessageArray[1]).ToLower();
                Boolean regDefender = _database.isUserRegistered(defender);
                Boolean regAttacker = _database.isUserRegistered(attacker);
                Boolean attackerEqualsDefender = attacker == defender;
                if (regAttacker && regDefender && !attackerEqualsDefender)
                {
                    int unitsAttacker = _database.selectUnits(attacker);
                    int unitsDefender = _database.selectUnits(defender);
                    int dicesAttacker = unitsAttacker / 10;
                    int dicesDefender = unitsDefender / 10;
                    if (dicesAttacker == 0) dicesAttacker = 1;
                    if (dicesDefender == 0) dicesDefender = 1;

                    int attackerFinalValue=0;
                    int defenderFinalValue=0;

                    for(int i = 0; i < dicesAttacker; i++)
                    {
                        attackerFinalValue = attackerFinalValue + DiceRoll();
                    }

                    for (int i = 0; i < dicesDefender; i++)
                    {
                        defenderFinalValue = defenderFinalValue + DiceRoll();
                    }

                    if (attackerFinalValue > defenderFinalValue)
                    {
                        int goldAttacker = _database.selectGold(attacker);
                        int goldDefender = _database.selectGold(defender);
                        _database.updateGold(attacker, goldAttacker + goldDefender / 3);
                        _database.updateGold(defender, goldDefender - goldDefender / 3);
                        res = "The attacker(with a roll of "+attackerFinalValue+") won and looted " + goldDefender / 3 + " gold from the defender (with a roll of "+defenderFinalValue+")!";
                    }
                    else
                    {
                        _database.updateUnits(attacker, unitsAttacker / 2);
                        res = "The defender won(with a roll of "+defenderFinalValue+") and the attacker lost " + unitsAttacker / 2 + " units (with a roll of "+attackerFinalValue+") !";
                    }

                }

            }
           
            return res;
        }

        private string Stats(IrcMessageData msg)
        {
            string res = null;
            string sender = RealName(msg.From).ToLower();
            Boolean senderRegistered = _database.isUserRegistered(sender);
            if (senderRegistered)
            {
                int playerUnits = _database.selectUnits(sender);
                int playerGold = _database.selectGold(sender);
                int playerMine = _database.selectMine(sender);
                int playerBarracks = _database.selectBarracks(sender);
                res = sender + " Your base has: " + playerGold + " gold, " + playerUnits + " units, lvl " + playerMine + " mine, " + playerBarracks + " barracks.";
            }
            return res;
        }

        private string UpgradeMine(IrcMessageData msg)
        {
            string res = null;
            string sender = RealName(msg.From).ToLower();
            Boolean senderRegistered = _database.isUserRegistered(sender);
            if (senderRegistered)
            {
                int playerMineLevel = _database.selectMine(sender);
                int playerGold = _database.selectGold(sender);
                if(playerGold >= playerMineLevel * 100)
                {
                    _database.updateMine(sender, playerMineLevel + 1);
                    _database.updateGold(sender, playerGold - (playerMineLevel * 100));
                    res = sender + "'s mine was upgraded to level " + (playerMineLevel+1) +".";
                }
                else
                {
                    res = sender + " you can't afford the upgrade .";
                }
                   
            }
            return res;
        }

        private string BuildUnits(IrcMessageData msg)
        {
            DumberLogger.Log(" CraftWarComp: User called buildUnits method");
            string res = null;
            string sender = RealName(msg.From).ToLower();
            Boolean senderRegistered = _database.isUserRegistered(sender);
           
            if(senderRegistered)
            {
                DumberLogger.Log(" CraftWarComp: User is regstered");
                Boolean barracksThere = false;
                int barracksLevel = _database.selectBarracks(sender);
                if (barracksLevel >= 1)
                {
                    barracksThere = true;
                }

                if (msg.MessageArray.Length >= 2 && barracksThere)
                {
                    DumberLogger.Log(" CraftWarComp: Message length is ok and barracks are there");
                    string unitCounts = msg.MessageArray[1];
                    long unitCountl = (long)Convert.ToDouble(unitCounts);
                    if (!(unitCountl < 0))
                    {
                        if (unitCountl <= 10000)
                        {
                            int senderGold = _database.selectGold(sender);
                            long sumToPay = unitCountl * 100;
                            if (sumToPay <= senderGold)
                            {
                                int unitssender = _database.selectUnits(sender);
                                _database.updateGold(sender, senderGold - (int)sumToPay);
                                _database.updateUnits(sender, unitssender + (int)unitCountl);
                                res = unitCountl + " units were added to your garrison " + sender + " .";
                            }
                            else
                            {
                                res = "You can't afford that many units " + sender + " .";
                            }
                        }
                        else
                        {
                            res = "You can only build max 10000 units at a time " + sender + " .";
                        }
                    }
                }
                else
                {
                    res = sender + " you don't have the barracks yet!";
                }
            }
            return res;
        }

        private string BuildBarracks(IrcMessageData msg)
        {
            string res = null;
            string sender = RealName(msg.From).ToLower();
            Boolean senderRegistered = _database.isUserRegistered(sender);
            Boolean racksAlreadyAvailable = _database.selectBarracks(sender) >= 1;
            if (senderRegistered&&!racksAlreadyAvailable)
            {
                int goldSender = _database.selectGold(sender);
                int price = 200;
                if (goldSender >= 200)
                {
                    _database.updateBarracks(sender, 1);
                    _database.updateGold(sender, goldSender - 200);
                    res = sender + " a Barracks has been built in your base .";
                }
                else
                {
                    res = sender + " you can't afford a barracks .";
                }
            }
            return res;
        }

        private string Maxupgrade(IrcMessageData msg)
        {
            string res = null;
            string sender = RealName(msg.From).ToLower();
            bool senderRegistered = _database.isUserRegistered(sender);
            if (senderRegistered)
            {
                int count=0;
                while (_database.selectGold(sender) > (_database.selectMine(sender)*100))
                {
                    int senderGold = _database.selectGold(sender);
                    int senderMineLevel = _database.selectMine(sender);
                    _database.updateGold(sender,senderGold - (senderMineLevel * 100));
                    _database.updateMine(sender, senderMineLevel + 1);
                    count++;
                }
                int finalMineLevel = _database.selectMine(sender);
                res = sender + " your mine has been upgraded " + count + " times to a final level of " + finalMineLevel+".";
            }
         
            return res;
        }

        private string MaxUnits(IrcMessageData msg)
        {
            string res = null;
            string sender = RealName(msg.From).ToLower();
            bool senderRegistered = _database.isUserRegistered(sender);
            if (senderRegistered)
            {
                var senderGold = _database.selectGold(sender);
                var senderUnits = _database.selectUnits(sender);
                var maxUnits = senderGold / 100;
                _database.updateGold(sender, senderGold - (maxUnits * 100));
                _database.updateUnits(sender, senderUnits + maxUnits);
                res = sender + " " + maxUnits + " were added to your garrison.";

            }
            return res;
        }

        private string RealName(string name)
        {
            string[] nameParts = name.Split('!');
            return nameParts[0];
        }

        private int DiceRoll()
        {
            int res = _randomObject.Next(1, 7);
            return res;
        }
    }
}
