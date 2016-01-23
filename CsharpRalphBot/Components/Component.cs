using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;


namespace CsharpRalphBot.Components
{
    abstract class Component
    {
        abstract public string Handle(IrcMessageData msg);
        abstract public Boolean Check(IrcMessageData msg);
        abstract public void ThreadTask();

    }
}
