using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpRalphBot
{
    static class DumberLogger
    {
        public static void log(string message)
        {
            string time = DateTime.Now.ToString("HH:mm:ss tt");
            Console.WriteLine(time + "||" + message);
        }
    }
}
