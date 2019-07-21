using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FinanceReporting.Controllers
{
    public class BankController : Controller
    {
        public IActionResult ViewAll()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
    }
}