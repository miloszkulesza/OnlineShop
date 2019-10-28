using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private IProductRepository productRepository;
        private Cart cartService;
        private UserManager<AppUser> userManager;

        public OrderController(IProductRepository productRepository,
            Cart cartService,
            UserManager<AppUser> userManager)
        {
            this.productRepository = productRepository;
            this.cartService = cartService;
            this.userManager = userManager;
        }

        public IActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateOrder(Order order)
        {
            return View();
        }
    }
}