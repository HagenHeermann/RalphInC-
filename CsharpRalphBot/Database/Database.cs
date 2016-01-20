using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CsharpRalphBot.Database
{
    class Database
    {
        private SQLiteConnection _connection;

        /// <summary>
        /// One time use only
        /// </summary>
        public void createDatabase()
        {
            SQLiteConnection.CreateFile("RalphsDatabase.sqlite");
            DumberLogger.log("Database: Database file created");
        }
    
        /// <summary>
        /// Method to connect to the database
        /// </summary>
        public void connectToDatabase()
        {
            _connection = new SQLiteConnection("Data Source=RalphsDatabase.sqlite;Version=3;");
            _connection.Open();
            DumberLogger.log("Database: Connection do database established");
        }

        /// <summary>
        /// One time use only
        /// </summary>
        public void createCraftWarTable()
        {
            string stmt = "CREATE TABLE CraftWar (USERNAME VARCHAR(100) PRIMARY KEY NOT NULL"+
                ",GOLD INT NOT NULL"+
                ",BARRACKS INT NOT NULL"+
                ",MINE INT NOT NULL+"+
                ",UNITS INT NOT NULL)";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            command.ExecuteNonQuery();
        }

        public void addPlayerToCraftWar(string username)
        {
            string stmt = "";
        }
        //Select Section

        public int selectGold(string username)
        {
            string stmt = "";
            return 0;
        }

        public int selectBarracks(string username)
        {
            string stmt = "";
            return 0;
        }

        public int selectMine(string username)
        {
            string stmt = "";
            return 0;
        }
        public int selectUnits(string username)
        {
            string stmt = "";
            return 0;
        }

        //Update Section

        public void updateGold(string username,int value)
        {
            string stmt = "";
        }

        public void updateBarracks(string username,int value)
        {
            string stmt = "";
        }

        public void updateMine(string username, int value)
        {
            string stmt = "";
        }

        public void updateUnits(string username,int value)
        {
            string stmt = "";
        }
    }
}
