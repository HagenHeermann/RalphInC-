using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using Meebey.SmartIrc4net;
using CsharpRalphBot.Database;

namespace CsharpRalphBot.Handler
{
    class RHandler
    {
        private Thread _handlerThread;
        private BlockingCollection<IrcMessageData> _messages;
        private Ralph _ralph;
        private List<Component> _components;

        public RHandler(Ralph _ralph)
        {
            _messages = new BlockingCollection<IrcMessageData>();
            this._ralph = _ralph;
            _handlerThread = new Thread(handle);
            _handlerThread.Start();
            _components = new List<Component>();
            CraftWarComp craftcomp = new CraftWarComp();
            _components.Add(craftcomp);
            DumberLogger.log("Handler: Handler created");
        }

        public void addMessage(object sender,IrcEventArgs args)
        {
            _messages.Add(args.Data);
            DumberLogger.log("Handler: Message added to q");
        }

        public void handle()
        {
            while (true)
            {
                IrcMessageData mToHandle = _messages.Take();
                Component[] comps = _components.ToArray();
                for(int i = 0; i < _components.Count(); i++)
                {
                    if (comps[i].check(mToHandle))
                    {
                        string msg = comps[i].handle(mToHandle);
                        if (msg != null)
                        {
                            _ralph.sendMessage(msg);
                        }
                        break;
                    }
                }                
            }
        }

    }

    
}

