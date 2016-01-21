using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.Generic;
namespace CsharpRalphBot.Database
{
    class RDatabase
    {
        private SQLiteConnection _connection;

        public RDatabase()
        {
            connectToDatabase();
        }
        /// <summary>
        /// One time use only
        /// working(tested)
        /// </summary>
        public void createDatabase()
        {
            SQLiteConnection.CreateFile("RalphsDatabase.sqlite");
            DumberLogger.log("Database: Database file created");
        }
    
        /// <summary>
        /// Method to connect to the database
        /// working(testet)
        /// </summary>
        public void connectToDatabase()
        {
            _connection = new SQLiteConnection("Data Source=RalphsDatabase.sqlite;Version=3;");
            _connection.Open();
            DumberLogger.log("Database: Connection do database established");
        }

        /// <summary>
        /// One time use only
        /// working(tested)
        /// </summary>
        public void createCraftWarTable()
        {
            string stmt = "CREATE TABLE CraftWar (USERNAME VARCHAR(100) PRIMARY KEY NOT NULL,GOLD INT NOT NULL,BARRACKS INT NOT NULL,MINE INT NOT NULL,UNITS INT NOT NULL)";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            command.ExecuteNonQuery();
            DumberLogger.log("Database: CraftWar table created");
        }

        /// <summary>
        /// Method to add a new player to the database
        /// working(tested)
        /// </summary>
        /// <param name="username"></param>
        public void addPlayerToCraftWar(string username)
        {
            string stmt = "INSERT INTO CraftWar (USERNAME,GOLD,BARRACKS,MINE,UNITS)VALUES('"+username+"',0,0,1,0)";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            command.ExecuteNonQuery();
            DumberLogger.log("Database: Player added to table CraftWar");
        }

        //Select Section
        /// <summary>
        /// Returns the gold value of a listed player
        /// working(tested)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int selectGold(string username)
        {
            string stmt = "SELECT GOLD FROM CraftWar WHERE USERNAME = '"+username+"'";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            int res = (int)reader["GOLD"];
            return res;
        }

        /// <summary>
        /// Returns the value set to the barracks 0 = no barracks else barracks exists
        /// working(tested)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int selectBarracks(string username)
        {
            string stmt = "SELECT BARRACKS FROM CraftWar WHERE USERNAME ='" + username + "'";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            int res = (int)reader["BARRACKS"];
            return res;
        }

        /// <summary>
        /// Returns the mine level of a listed player
        /// working (tested)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int selectMine(string username)
        {
            string stmt = "SELECT MINE FROM CraftWar WHERE USERNAME ='" + username + "'";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            int res = (int)reader["MINE"];
            return res;
        }

        /// <summary>
        /// Returns the units of a listed player
        /// working (tested)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int selectUnits(string username)
        {
            string stmt = "SELECT UNITS FROM CraftWar WHERE USERNAME ='" + username + "'";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            int res = (int)reader["UNITS"];
            return res;
        }

        //Update Section

        /// <summary>
        /// Method to update the gold of a listed player
        /// works (tested)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="value"></param>
        public void updateGold(string username,int value)
        {
            string stmt = "UPDATE CraftWar SET GOLD ="+value+" WHERE USERNAME='" + username + "'";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Method to update the barracks of a listed player
        /// </summary>
        /// <param name="username"></param>
        /// <param name="value"></param>
        public void updateBarracks(string username,int value)
        {
            string stmt = "UPDATE CraftWar SET BARRACKS =" + value + " WHERE USERNAME='" + username + "'";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Method to update the Mine level of a listed player
        /// </summary>
        /// <param name="username"></param>
        /// <param name="value"></param>
        public void updateMine(string username, int value)
        {
            string stmt = "UPDATE CraftWar SET MINE =" + value + " WHERE USERNAME='" + username + "'";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Method to update the Unit number of a listed player
        /// </summary>
        /// <param name="username"></param>
        /// <param name="value"></param>
        public void updateUnits(string username,int value)
        {
            string stmt = "UPDATE CraftWar SET UNITS =" + value + " WHERE USERNAME='" + username + "'";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Method to get all registered players
        /// </summary>
        /// <returns></returns>
        public string[] getAllPlayers()
        {
            List<string> resultB = new List<string>();
            string stmt = "SELECT USERNAME FROM CraftWar";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                resultB.Add((string)reader["USERNAME"]);
            }
            string[] result = resultB.ToArray();
            return result;
        }
        /// <summary>
        /// Method to check if a player is registered
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Boolean isUserRegistered(string username)
        {
            Boolean res = false;
            string[] playerlist = getAllPlayers();
            for(int i = 0; i < playerlist.Length; i++)
            {
                if (username == playerlist[i].ToLower()) res = true;
            }
                    
            return res;
        }

        /// <summary>
        /// Use this method do delete a player from the game
        /// </summary>
        /// <param name="username"></param>
        public void dropUser(string username)
        {
            string stmt = "DELETE FROM CraftWar WHERE USERNAME='" + username + "'";
            SQLiteCommand command = new SQLiteCommand(stmt, _connection);
            command.ExecuteNonQuery();
        }
    }
}
