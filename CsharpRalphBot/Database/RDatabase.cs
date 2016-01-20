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
            DumberLogger.log("Database: CraftWar table created");
        }

        /// <summary>
        /// Method to add a new player to the database
        /// </summary>
        /// <param name="username"></param>
        public void addPlayerToCraftWar(string username)
        {
            string stmt = "";
        }

        //Select Section
        /// <summary>
        /// Returns the gold value of a listed player
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int selectGold(string username)
        {
            string stmt = "";
            return 0;
        }

        /// <summary>
        /// Returns the value set to the barracks 0 = no barracks else barracks exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int selectBarracks(string username)
        {
            string stmt = "";
            return 0;
        }

        /// <summary>
        /// Returns the mine level of a listed player
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int selectMine(string username)
        {
            string stmt = "";
            return 0;
        }

        /// <summary>
        /// Returns the units of a listed player
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int selectUnits(string username)
        {
            string stmt = "";
            return 0;
        }

        //Update Section

        /// <summary>
        /// Method to update the gold of a listed player
        /// </summary>
        /// <param name="username"></param>
        /// <param name="value"></param>
        public void updateGold(string username,int value)
        {
            string stmt = "";
        }

        /// <summary>
        /// Method to update the barracks of a listed player
        /// </summary>
        /// <param name="username"></param>
        /// <param name="value"></param>
        public void updateBarracks(string username,int value)
        {
            string stmt = "";
        }

        /// <summary>
        /// Method to update the Mine level of a listed player
        /// </summary>
        /// <param name="username"></param>
        /// <param name="value"></param>
        public void updateMine(string username, int value)
        {
            string stmt = "";
        }

        /// <summary>
        /// Method to update the Unit number of a listed player
        /// </summary>
        /// <param name="username"></param>
        /// <param name="value"></param>
        public void updateUnits(string username,int value)
        {
            string stmt = "";
        }
    }
}
