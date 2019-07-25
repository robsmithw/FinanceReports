using System;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace FinanceReporting.Models
{
    public class DatabaseModel
    {
        private DatabaseModel()
        {
        }
        private string database = String.Empty;
        public string Database
        {
            get { return database; }
            set { database = value; }
        }
        public string Password { get; set; }
        public string User { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }
        private static DatabaseModel _instance = null;
        public static DatabaseModel Instance()
        {
            _instance = new DatabaseModel();
            return _instance;
        }
        public bool IsConnected()
        {
            if(Connection == null)
            {
                if (String.IsNullOrEmpty(database))
                {
                    return false;
                }
                string connString = string.Format("Server=192.168.1.226; database={0}; UID={1}; password={2}", database, User, Password);
                connection = new MySqlConnection(connString);
                connection.Open();
            }
            return true;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
