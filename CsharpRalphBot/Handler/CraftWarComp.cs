using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;


namespace CsharpRalphBot.Handler
{
    class CraftWarComp : Component
    {
        private string[] commands = {"#attack","#stats","#commands","#upgrade","#buildBarracks","#buildUnits","#add"};

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
                    res = addPlayer();
                    break;
                default:
                    res = null;
                    break;
            }
            return res;
        }

        private string addPlayer()
        {
            return "player add";
        }

        private string getCommands()
        {
            return "commands";
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
    }
}
