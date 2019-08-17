using System;
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
            var context = new FinanceReportingContext();
            var hashFromDb = context.GetHash(username);
            if(!String.IsNullOrEmpty(hashFromDb) && hashFromDb == hash)
            {
                authenticated = true;
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
            var hash = ToMD5(reqPass);
            var firstname = model.firstName;
            var lastname = model.lastName;
            var emailAddress = model.email;
            if(CreatedAccount(reqUser, hash, firstname, lastname, emailAddress))
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public bool CreatedAccount(string reqUsername, string reqPassword, string firstname, string lastname, string emailAdress)
        {
            var created = false;
            var context = new FinanceReportingContext();
            var returnMessage = context.CreateAccount(reqUsername, reqPassword, firstname, lastname, emailAdress);
            if (!String.IsNullOrEmpty(returnMessage) && returnMessage == "Successfully created user.")
            {
                created = true;
            }
            return created;
        }
    }
}