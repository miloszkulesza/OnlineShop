﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", new { controller = "Catalog" });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Tutaj będą informacje o firmie.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Tutaj będą dane kontaktowe.";

            return View();
        }

        public IActionResult Regulations()
        {
            ViewData["Message"] = "Tutaj będzie regulamin sklepu";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
