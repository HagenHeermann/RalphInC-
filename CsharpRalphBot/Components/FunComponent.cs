using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;

namespace CsharpRalphBot.Components
{
    class FunComponent : Component
    {
        private string[] commands = {"#gamba"};

        public override bool Check(IrcMessageData msg)
        {
            bool res = false;
            string com = msg.MessageArray[0];
            for (int i = 0; i < commands.Length; i++)
            {
                if (com == commands[i]) res = true;
            }
            return res;
        }

        public override string Handle(IrcMessageData msg)
        {
            string res = null;
            string com = msg.MessageArray[0];
            switch (com)
            {
                case "#gamba":
                    res = Gamba(msg);
                    break;
            }
            return res;
            

        }

        public override void ThreadTask()
        {
            throw new NotImplementedException();
        }

        private string Gamba(IrcMessageData msg)
        {
            string sender = RealName(msg.From).ToLower();
            string res = sender + " GAMBA FeelsGoodMan !!!";
            return res;
        }

        private string RealName(string name)
        {
            string[] nameParts = name.Split('!');
            return nameParts[0];
        }



    }
}
