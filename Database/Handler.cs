using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Database
{
    public static class DatabaseHandler
    {
        // Helper function to get a connection
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection("server=localhost;user=root;database=life;port=3306;password=root");
        }

        // If the tables were generated succesfully or already exist, return true.
        public static void GenerateTables()
        {
            Query query = new Query(QueryType.CreateTable, "characters");
            query.AddColumn("character_id", "INT(11) UNSIGNED NOT NULL AUTO_INCREMENT");
            query.AddColumn("player_id", "INT(11) UNSIGNED NOT NULL");
            query.AddColumn("inventory_id", "INT(11) UNSIGNED NOT NULL");
            query.AddColumn("name", "TEXT DEFAULT NULL");
            query.SetPrimaryKey("character_id");
            query.Execute();
        }
    }
}
