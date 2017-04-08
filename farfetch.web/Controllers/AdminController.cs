using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace farfetch.web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult items()
        {
            return View();
        }

        public IActionResult categories()
        {
            return View();
        }

        public IActionResult orders()
        {
            return View();
        }
    }
}