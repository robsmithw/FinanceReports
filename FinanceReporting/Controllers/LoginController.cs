using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceReporting.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FinanceReporting.Controllers
{
    public class LoginController : Controller
    {
        //Default Login Page
        public IActionResult Login()
        {
            return View();
        }

        //Will come back later for error checking
        [HttpPost]
        public IActionResult Login(UserViewModel model)
        {
            var md5Hash = ToMD5(model.password);
            if(UserAuthed(model.username, md5Hash))
            {
                return View("Home");
            }
            return View();
        }

        public bool UserAuthed(string username, string hash)
        {
            var authenticated = false;
            var dbCon = DatabaseModel.Instance();

            //will eventually have a way to not have these hard coded
            dbCon.User = "root";
            dbCon.Password = "Password1";
            dbCon.Database = "FinanceReporting";
            if (dbCon.IsConnected())
            {
                var query = String.Format("CALL `usp_AuthenticateUser`('{0}');", username);
                var hashFromDb = String.Empty;
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    hashFromDb = reader.GetString(0);
                }
                dbCon.Close();
                if(!String.IsNullOrEmpty(hashFromDb) && hashFromDb == hash)
                {
                    authenticated = true;
                }
            }
            return authenticated;
        }

        public static string ToMD5(string password)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        //Will come back later for error checking
        [HttpPost]
        public IActionResult Register(UserViewModel model)
        {
            var reqUser = model.username;
            //need to check for 8 characters, a number, and special character
            var reqPass = model.password;
            var firstname = model.firstName;
            var lastname = model.lastName;
            var emailAddress = model.email;
            if(CreatedAccount(reqUser, reqPass, firstname, lastname, emailAddress))
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public bool CreatedAccount(string reqUsername, string reqPassword, string firstname, string lastname, string emailAdress)
        {
            var created = false;
            var dbCon = DatabaseModel.Instance();

            //will eventually have a way to not have these hard coded
            dbCon.User = "root";
            dbCon.Password = "Password1";
            dbCon.Database = "FinanceReporting";
            if (dbCon.IsConnected())
            {
                var query = String.Format("CALL `usp_CreateAccount`('{0}','{1}','{2}','{3}','{4}');", reqUsername, reqPassword, firstname,lastname,emailAdress);
                var returnMessage = String.Empty;
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    returnMessage = reader.GetString(0);
                }
                dbCon.Close();
                if (!String.IsNullOrEmpty(returnMessage) && returnMessage == "Successfully created user.")
                {
                    created = true;
                }
            }
            return created;
        }
    }
}