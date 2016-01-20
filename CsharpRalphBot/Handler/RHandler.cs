using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using Meebey.SmartIrc4net;

namespace CsharpRalphBot.Handler
{
    class RHandler
    {
        private Thread _handlerThread;
        private BlockingCollection<IrcMessageData> _messages;
        private Ralph _ralph;

        public RHandler(Ralph _ralph)
        {
            this._ralph = _ralph;
            _handlerThread = new Thread(handle);
            _handlerThread.Start();
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
                string[] splittedM = mToHandle.Message.Split(' ');
                if (splittedM[0] == "\test")
                {
                    _ralph.sendMessage("test");
                }
                
            }
        }

    }

    
}

