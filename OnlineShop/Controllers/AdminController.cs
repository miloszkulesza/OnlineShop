using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Administrator, Pracownik")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        public IActionResult Categories()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Roles()
        { 
            return View();
        }


        public IActionResult Users()
        {
            return View();
        }

        public IActionResult Orders()
        {
            return View();
        }
    }
}