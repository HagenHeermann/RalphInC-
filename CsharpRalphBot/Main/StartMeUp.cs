using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpRalphBot
{
    class StartMeUp
    {
        static void Main(string[] args)
        {
            Boolean alive=true;
            string nameIn;
            string tokenIn;
            string channelIn;
            string end;
            Console.WriteLine("enter token: ");
            tokenIn = Console.ReadLine();
            Console.WriteLine("enter name: ");
            nameIn = Console.ReadLine();
            Console.WriteLine("enter channel: ");
            channelIn = Console.ReadLine();

            Ralph ralph = new Ralph(channelIn, tokenIn, nameIn);
            ralph.connectRalph();
            ralph.sendMessage("MingLee");

            while (alive)
            {
                end = Console.ReadLine();
                if (end == "kill")
                {
                    alive = false;
                }
            }

        }

    }
}
