﻿/*
* The Core class for the bot
*
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;


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

        public Ralph(String _channel,String _token,String _name)
        {
            this._token = _token;
            this._channel = _channel;
            this._name = _name;
            _client = new IrcClient()
            {
                Encoding = Encoding.UTF8,
                ActiveChannelSyncing = true,
                SendDelay = 200,
                AutoRejoin = true,
                AutoRejoinOnKick = true,
                AutoReconnect = true,
                AutoRelogin = true,
                AutoRetry = true,
                AutoRetryLimit = 0
            };
            DumberLogger.log("Ralph: ralph got created");
        }
        /// <summary>
        /// /Method to connect ralph to the wished channel.
        /// </summary>
        public void connectRalph()
        {
            _client.Connect(Server, portnum);
            DumberLogger.log("Ralph: connected to server");
            _client.Login(_name, _name, 0, _name, _token);
            DumberLogger.log("Ralph: logged in");
        }

        public void sendMessage(string message)
        {
            _client.SendMessage(SendType.Message, _channel, message);
            DumberLogger.log("Ralph: message send");
        }
        

    }
}
