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

        public CraftWarComp()
        {
            _database = new RDatabase();
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
                    res = buildBarracks();
                    break;
                case "#buildUnits":
                    res = buildUnits();
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
                    res = "User " + userToAdd + " added to registered players";
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
                        res = "The attacker won and looted " + goldDefender / 3 + " gold";
                    }
                    else
                    {
                        _database.updateUnits(attacker, unitsAttacker / 2);
                        res = "The defender won and the attacker lost " + unitsAttacker / 2 + " units";
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
                res = sender + " Your base has: " + playerGold + " gold, " + playerUnits + " units, " + playerMine + " lvl mine, " + playerBarracks + " barracks.";
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
                    res = sender + "'s mine was upgraded to level " + playerMineLevel + 1+".";
                }
                else
                {
                    res = sender + " you dont have enough gold to upgrade your mine!";
                }
                   
            }
            return res;
        }

        private string buildUnits()
        {
            return "units";
        }

        private string buildBarracks()
        {
            return "barracks";
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
