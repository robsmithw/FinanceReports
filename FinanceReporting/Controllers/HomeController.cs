using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using FinanceReporting.Models;

namespace FinanceReporting.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            var dbCon = DatabaseModel.Instance();
            dbCon.User = "root";
            dbCon.Password = "Password1";
            dbCon.Database = "FinanceReporting";
            var user = "rsmith";
            if (dbCon.IsConnected())
            {
                var query = String.Format("CALL `usp_AuthenticateUser`('{0}');",user);
                
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string someStringFromColumnZero = reader.GetString(0);
                    ViewData["Message"] = String.Format("{0}'s password is {1}",user,someStringFromColumnZero);
                }
                dbCon.Close();
            }
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
