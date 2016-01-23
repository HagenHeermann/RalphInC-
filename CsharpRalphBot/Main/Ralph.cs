/*
* The Core class for the bot
*
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;
using CsharpRalphBot.Handler;
using System.Threading;
using CsharpRalphBot.Handler;
using CsharpRalphBot.Database;

namespace CsharpRalphBot
{
    class Ralph
    {
        private const string Server = "irc.twitch.tv";
        private string _name;
        private string _channel;
        private string _token;
        private const int portnum = 6667;
        private IrcClient _client;
        private RHandler _handler;

        public Ralph(String _channel,String _token,String _name)
        {
            
            this._token = "oauth:"+_token;
            this._channel = _channel;
            this._name = "AneleBot";

            _client = new IrcClient()
            {
                Encoding = Encoding.UTF8,
                ActiveChannelSyncing = true,
                SendDelay = 5000,
                AutoRejoin = true,
                AutoRejoinOnKick = true,
                AutoReconnect = true,
                AutoRelogin = true,
                AutoRetry = true,
                AutoRetryLimit = 0
            };
            _client.OnChannelMessage+=onMessage;
            _client.OnRawMessage += logMessage;
            _handler = new RHandler(this);

            DumberLogger.Log("Ralph: ralph got created");
        }

        public void connectRalph()
        {
            _client.Connect(Server, portnum);
            DumberLogger.Log("Ralph: connected to server");
            _client.Login(_name, _name, 0, _name, _token);
            DumberLogger.Log("Ralph: logged in");
            _client.RfcJoin(_channel);
            DumberLogger.Log("" + _client.IsJoined("ashwinitv"));
            _client.Listen();
        }        

        public void sendMessage(string message)
        {
            _client.SendMessage(SendType.Message, _channel, message);
            DumberLogger.Log("Ralph: message send");
        }

        public void onMessage(object sender,IrcEventArgs args)
        {
            DumberLogger.Log("Ralph: trying to add message to the handler");
            _handler.addMessage(sender, args);
        }

        public void logMessage(object sender,IrcEventArgs args)
        {
            IrcMessageData mess = args.Data;
            DumberLogger.Log(mess.Message);
        }

    }

}
