using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceReporting.Models
{
    public class FinanceReportingContext
    {
        private const string DATABASE_NAME = "FinanceReporting";
        public string GetHash(string username)
        {
            var hashFromDb = String.Empty;
            var dbCon = DatabaseModel.Instance();
            //will eventually have a way to not have these hard coded
            dbCon.User = "root";
            dbCon.Password = "Password1";
            dbCon.Database = DATABASE_NAME;
            if (dbCon.IsConnected())
            {
                var query = String.Format("CALL `usp_AuthenticateUser`('{0}');", username);
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    hashFromDb = reader.GetString(0);
                }
                dbCon.Close();
            }
            return hashFromDb;
        }

        public string CreateAccount(string reqUsername, string hash, string firstname, string lastname, string emailAdress)
        {
            var returnMessage = String.Empty;
            var dbCon = DatabaseModel.Instance();
            //will eventually have a way to not have these hard coded
            dbCon.User = "root";
            dbCon.Password = "Password1";
            dbCon.Database = DATABASE_NAME;
            if (dbCon.IsConnected())
            {
                var query = String.Format("CALL `usp_CreateAccount`('{0}','{1}','{2}','{3}','{4}');", reqUsername, hash, firstname,
                    lastname, emailAdress);
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    returnMessage = reader.GetString(0);
                }
                dbCon.Close();
            }
            return returnMessage;
        }
    }
}
