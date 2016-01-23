using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;

namespace CsharpRalphBot.Components
{
    class UtilComponent : Component
    {
        private string[] commands = { "#disconnect" };

        public override bool Check(IrcMessageData msg)
        {
            Boolean res = false;
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
                case "#disconnect":
                    Disconnect(msg);
                    break;
                default:
                    break;
            }
            return res;
        }

        public override void ThreadTask()
        {
        }

        private string Disconnect(IrcMessageData msg)
        {
            string sender = RealName(msg.From).ToLower();
            bool isSenderDev = sender == "voodoohood" || sender == "n1ghtsh0ck";
            if (isSenderDev)
            {
                Environment.Exit(0);
            }
            
            return null;
        }

        private string RealName(string name)
        {
            string[] nameParts = name.Split('!');
            return nameParts[0];
        }

    }
}
