using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CsharpRalphBot.Database;

namespace CsharpRalphBot.Handler
{
    class CraftWarCompThread
    {

        private Thread _craftWarUpdater;
        private RDatabase _database;

        public CraftWarCompThread(RDatabase databse)
        {
            _craftWarUpdater = new Thread(update);
            _database = databse;
        }

        public void start()
        {
            _craftWarUpdater.Start();
        }

        private void update()
        {
            while (1<2)
            {
                DumberLogger.log(" CraftWarCompThread: started update");
                string[] regUsers = _database.getAllPlayers();
                for(int i = 0; i < regUsers.Length; i++)
                {
                    int goldUser = _database.selectGold(regUsers[i]);
                    int mineLevelUser = _database.selectMine(regUsers[i]);
                    _database.updateGold(regUsers[i], goldUser + mineLevelUser * 10);
                }
                DumberLogger.log(" CraftWarCompThreada: sleeping for two minutes");
                Thread.Sleep(60000);
            }
           
        }
    }
}
