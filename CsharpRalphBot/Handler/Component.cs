using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;


namespace CsharpRalphBot.Handler
{
    abstract class Component
    {
        abstract public string handle(IrcMessageData msg);
        abstract public Boolean check(IrcMessageData msg);
        abstract public void ThreadTask();
        
    }
}
