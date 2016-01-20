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
                    res = attack();
                    break;
                case "#stats":
                    res = stats();
                    break;
                case "#commands":
                    res = getCommands();
                    break;
                case "#upgrade":
                    res = upgradeMine();
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


        private string attack()
        {
            return "attack";
        }

        private string stats()
        {
            return "stats";
        }

        private string upgradeMine()
        {
            return "mine";
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
    }
}
