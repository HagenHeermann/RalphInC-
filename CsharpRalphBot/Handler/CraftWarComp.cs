using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;
using CsharpRalphBot.Database;


namespace CsharpRalphBot.Handler
{
    class CraftWarComp : Component
    {
        private string[] commands = {"#attack","#stats","#commands","#upgrade","#buildBarracks","#buildUnits","#add"};
        private RDatabase _database;
        private CraftWarCompThread craftWarThread;

        public CraftWarComp()
        {
            _database = new RDatabase();
            craftWarThread = new CraftWarCompThread(_database);
            craftWarThread.start();
        }

        public override void ThreadTask()
        {
            throw new NotImplementedException();
        }

        public override bool check(IrcMessageData msg)
        {
            Boolean res = false;
            string com = msg.MessageArray[0];
            for(int i = 0; i < commands.Length; i++)
            {
                if (com == commands[i]) res = true;
            }
            return res;
        }

        public override string handle(IrcMessageData msg)
        {
            string res;
            string com = msg.MessageArray[0];
            switch (com)
            {
                case "#attack":
                    res = attack(msg);
                    break;
                case "#stats":
                    res = stats(msg);
                    break;
                case "#commands":
                    res = getCommands();
                    break;
                case "#upgrade":
                    res = upgradeMine(msg);
                    break;
                case "#buildBarracks":
                    res = buildBarracks(msg);
                    break;
                case "#buildUnits":
                    res = buildUnits(msg);
                    break;
                case "#add":
                    res = addPlayer(msg);
                    break;
                default:
                    res = null;
                    break;
            }
            return res;
        }

        private string addPlayer(IrcMessageData msg)
        {
            string res;
            string sender = realName(msg.From).ToLower();
            if (sender == "voodoohood")
            {
                if(msg.MessageArray.Length >= 2)
                {
                    string userToAdd = msg.MessageArray[1].ToLower();
                    _database.addPlayerToCraftWar(userToAdd);
                    res = "User " + userToAdd + " added to the registered players .";
                    DumberLogger.log(" CraftWarComp: Added user" + userToAdd + " to database, is registered " + _database.isUserRegistered(userToAdd));  
                }
                else
                {
                    res = null;
                    DumberLogger.log(" CraftWarComp: Format of the message wasnt correct");
                }
            }
            else
            {
                res = null;
                DumberLogger.log(" CraftWarComp: User other than voodoohood tryed to add a user");
            }
            return res;
        }

        private string getCommands()
        {
            string res="";
            for(int i = 0; i < commands.Length; i++)
            {
                res = res +" | "+ commands[i];
            }
            return res;
        }


        private string attack(IrcMessageData msg)
        {
            String res = null;
            if (msg.MessageArray.Length >= 2)
            {
                string attacker = realName(msg.From).ToLower();
                string defender = realName(msg.MessageArray[1]).ToLower();
                Boolean regDefender = _database.isUserRegistered(defender);
                Boolean regAttacker = _database.isUserRegistered(attacker);
                Boolean attackerEqualsDefender = attacker == defender;
                if (regAttacker && regDefender && !attackerEqualsDefender)
                {
                    int unitsAttacker = _database.selectUnits(attacker);
                    int unitsDefender = _database.selectUnits(defender);
                    int dicesAttacker = unitsAttacker / 100;
                    int dicesDefender = unitsDefender / 100;

                    int attackerFinalValue=0;
                    int defenderFinalValue=0;

                    for(int i = 0; i < dicesAttacker; i++)
                    {
                        attackerFinalValue = attackerFinalValue + diceRoll();
                    }

                    for (int i = 0; i < dicesDefender; i++)
                    {
                        defenderFinalValue = defenderFinalValue + diceRoll();
                    }

                    if (attackerFinalValue > defenderFinalValue)
                    {
                        int goldAttacker = _database.selectGold(attacker);
                        int goldDefender = _database.selectGold(defender);
                        _database.updateGold(attacker, goldAttacker + goldDefender / 3);
                        _database.updateGold(defender, goldDefender - goldDefender / 3);
                        res = "The attacker won and looted " + goldDefender / 3 + " gold !";
                    }
                    else
                    {
                        _database.updateUnits(attacker, unitsAttacker / 2);
                        res = "The defender won and the attacker lost " + unitsAttacker / 2 + " units !";
                    }

                }

            }
           
            return res;
        }

        private string stats(IrcMessageData msg)
        {
            string res = null;
            string sender = realName(msg.From).ToLower();
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

        private string upgradeMine(IrcMessageData msg)
        {
            string res = null;
            string sender = realName(msg.From).ToLower();
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

        private string buildUnits(IrcMessageData msg)
        {
            DumberLogger.log(" CraftWarComp: User called buildUnits method");
            string res = null;
            string sender = realName(msg.From).ToLower();
            Boolean senderRegistered = _database.isUserRegistered(sender);
           
            if(senderRegistered)
            {
                DumberLogger.log(" CraftWarComp: User is regstered");
                Boolean barracksThere = false;
                int barracksLevel = _database.selectBarracks(sender);
                if (barracksLevel >= 1)
                {
                    barracksThere = true;
                }

                if (msg.MessageArray.Length >= 2 && barracksThere)
                {
                    DumberLogger.log(" CraftWarComp: Message length is ok and barracks are there");
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

        private string buildBarracks(IrcMessageData msg)
        {
            string res = null;
            string sender = realName(msg.From).ToLower();
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

        private string realName(string name)
        {
            string[] nameParts = name.Split('!');
            return nameParts[0];
        }

        private int diceRoll()
        {
            Random rnd = new Random();
            int res = rnd.Next(1, 7);
            return res;
        }
    }
}
