using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;
using CsharpRalphBot.Database;
using System.Threading;

namespace CsharpRalphBot.Components
{
    class TwitchCasinoComponent : Component
    {
        private bool _rouletteActive;
        private bool _casinoOpen;
        private RDatabase _database;
        private string[] commands = { "#startRoulette", "#placebet","#openCasino","#closeCasino" };
        private Ralph _ralph;
        private Random _random;

        public TwitchCasinoComponent(Ralph ralph)
        {
            _casinoOpen = false;
            _random = new Random();
            _ralph = ralph;
            _rouletteActive = false;
            _database = new RDatabase();
        }

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

        public override void ThreadTask()
        {
            throw new NotImplementedException();
        }

        public override string Handle(IrcMessageData msg)
        {
            string res;
            string com = msg.MessageArray[0];
            switch (com)
            {
                case "#startRoulette":
                    res = StartRoulette(msg);
                    break;
                case "#placebet":
                    res = AddBet(msg);
                    break;
                default:
                    res = null;
                    break;
            }
            return res;
        }

        //Roulette
        private int _roll;
        private List<bet> _bets;

        private string StartRoulette(IrcMessageData msg)
        {
            
            string res = null;
            string sender = RealName(msg.From).ToLower();
            if(sender == "voodoohood")
            {
                _rouletteActive = true;
                res = "Roulette started place your bets , you have 1 minute time. Use #placebet <0-36> <1-10000> to place a bet";
                Thread rouletteThread = new Thread(RouletteThreadTask);
                _bets = new List<bet>();
                rouletteThread.Start();
            }
            return res;
        }

        private string AddBet(IrcMessageData msg)
        {
            string res = null;
            string sender = RealName(msg.From).ToLower();
            bool isRegistered = _database.isUserRegistered(sender);
            if (isRegistered&&_rouletteActive)
            {
                DumberLogger.Log(" Casino: try to add bett / Roulette started / User is registered");
                bool messageCorrectLength = msg.MessageArray.Length>=3;
                if (messageCorrectLength)
                {
                    DumberLogger.Log(" Casino: The Bet add call has the correct length");
                    string numberS = msg.MessageArray[1];
                    long number = (long)Convert.ToDouble(numberS);
                    string valueS = msg.MessageArray[2];
                    long value = (long)Convert.ToDouble(valueS);
                    bool rangeMax = number < 37;
                    bool rangeMin = number >= 0;
                    bool maxBet = value < 10000;
                    bool minBet = value > 0;
                    bool userHasEnoughGold = value <= _database.selectGold(sender);
                    bool userHasAlreadyTooMuchOnTheTable = checkBets(sender);
                    DumberLogger.Log(" Casino: " + number + " " + value + " " + rangeMax + " " + rangeMin + " " + maxBet + " " + minBet + " " + userHasEnoughGold + " " + userHasAlreadyTooMuchOnTheTable);
                    if (rangeMax && rangeMin && maxBet && minBet && userHasEnoughGold&&userHasAlreadyTooMuchOnTheTable)
                    {
                        DumberLogger.Log(" Casino: range ok / max and min bet ok / the user has enough gold / doesn have too much on the table");
                        bet userBet = new bet(sender, (int)number, (int)value);
                        _bets.Add(userBet);
                        int usersGold = _database.selectGold(sender);
                        _database.updateGold(sender, usersGold - (int)value);
                        res = sender + " your bet of " + value + " gold on number " + number + " has been confirmed";
                    }

                }
            }
            return res;
        }

        private bool checkBets(string sender)
        {
            bool res = false;
            int bettedgold = 0;
            for (int i = 0; i < _bets.Count; i++)
            {
                if(_bets[i].sender == sender)
                {
                    bettedgold = bettedgold + _bets[i].value;
                }
            }
            if(bettedgold <= 10000)
            {
                res = true;
            }
            return res;
        }

        private string RealName(string name)
        {
            string[] nameParts = name.Split('!');
            return nameParts[0];
        }

        private void RouletteThreadTask()
        {
            Thread.Sleep(60000);
            _rouletteActive = false;
            RouletteEnd();
            
        }

        private void RouletteEnd()
        {
            DumberLogger.Log(" Casino: Roulette ended");
            RouletteRoll();
            string message = "The roll was "+ _roll +" The winners are: ";
            bool nobodyWon = true;
            for(int i = 0; i < _bets.Count; i++)
            {
                if(_bets[i].number == _roll)
                {
                    nobodyWon = false;
                    string player = _bets[i].sender;
                    int userGold = _database.selectGold(player);
                    _database.updateGold(player, userGold + _bets[i].value * 2);
                    message = message + player + "(" + (_bets[i].value * 2) + "), ";
                }
            }
            _ralph.sendMessage(message);
        }

        private void RouletteRoll()
        {
            int number = _random.Next(0, 37);
            _roll = number;
        }

    }

    public struct bet
    {
        public string sender { get; set; }
        public int number { get; set; }
        public int value { get; set; }

        public bet(string sender,int number,int value)
        {
            this.sender = sender;
            this.number = number;
            this.value = value;
        }
    }
}
