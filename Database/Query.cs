using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Database
{
    // Implemented query types
    public enum QueryType
    {
        CreateTable,
        Insert,
        Select
    }

    // Used for WHERE statements.
    public enum ExpressionType
    {
        Like,
        Equals
    }

    public class Query
    {
        private List<string[]> columns = new List<string[]>();
        private string table;
        private string primaryKey;
        private bool whereResult;
        private QueryType queryType;

        // Constructor to create a new query with type
        public Query(QueryType type, string tab)
        {
            queryType = type;
            table = tab;
        }

        // Helper function to check what type of query this is.
        public bool IsType(QueryType type)
        {
            return (queryType == type);
        }

        // Used for CreateTable
        public void AddColumn(string key, string type)
        {
            if (IsType(QueryType.CreateTable))
            {
                string[] column = { key, type };
                columns.Add(column);
            }
        }

        public void Insert(string key, string data)
        {
            if (IsType(QueryType.Insert))
            {
                string[] column = { key, data };
                columns.Add(column);
            }
        }

        public void Where(string key, ExpressionType expression, string argument)
        {
            if (IsType(QueryType.Insert) || IsType(QueryType.Select))
            {

            }
        }

        public bool IsTrue()
        {
            return whereResult;
        }

        // Used for CreateTable
        // todo: check if key exists
        public void SetPrimaryKey(string key)
        {
            primaryKey = key;
        }

        public void Execute()
        {
            switch (queryType)
            {
                case QueryType.CreateTable:
                    // Check if the table actually exists before attempting to create it.
                    if (!TableExists(table))
                    {
                        MPUtil.WriteToConsole($"Creating table: {table}");
                        string cols = "";

                        for (int i = 0; i < columns.Count; i++)
                        {
                            cols = cols + columns[i][0] + " " + columns[i][1] + ",";
                        }

                        ExecuteRawNoRead($@" CREATE TABLE `{table}` ({cols} PRIMARY KEY ({primaryKey})) COLLATE='utf8_general_ci' ENGINE=InnoDB;");
                    }
                    else
                    {
                        MPUtil.WriteToConsole($"Loading table: {table}");
                    }
                    break;
                case QueryType.Insert:
                    string keys = "";
                    string values = "";

                    for (int i = 0; i < columns.Count; i++)
                    {
                        keys = keys + columns[i][0];
                        values = values + $"'{columns[i][1]}'";

                        if (i < columns.Count - 1)
                        {
                            keys = keys + ",";
                            values = values + ",";
                        }

                    }

                    ExecuteRawNoRead($"INSERT INTO {table}({keys}) VALUES({values})");
                    break;
                default:
                    // do nothing
                    break;
            }
        }

        private static void ExecuteRawNoRead(string query)
        {
            MySqlConnection connection = DatabaseHandler.GetConnection();
            try
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MPUtil.WriteToConsole(ex.ToString());
            }

            connection.Close();
        }

        // Helper function to check if a table exists
        private static bool TableExists(string table)
        {
            MySqlConnection connection = DatabaseHandler.GetConnection();
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand($"SELECT * FROM {table};", connection);

                command.ExecuteNonQuery();
            }
            catch
            {
                connection.Close();
                return false;
            }

            connection.Close();
            return true;
        }
    }
}
