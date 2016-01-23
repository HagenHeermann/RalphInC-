using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace CsharpRalphBot.Handler
{
    class ComponentThreadsHandler
    {
        List<Thread> _componentThreads;

        public ComponentThreadsHandler()
        {
            _componentThreads = new List<Thread>();
        }

    }
}
