using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace farfetch.web.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index(int? id)
        {
            return View();
        }
    }
}