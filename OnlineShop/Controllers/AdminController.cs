using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Administrator, Pracownik")]
    public class AdminController : Controller
    {
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        private IOrderRepository orderRepository;
        private IUserRolesRepository userRolesRepository;
        private IOrderPositionRepository orderPositionRepository;

        public AdminController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            IOrderRepository orderRepository,
            IUserRolesRepository userRolesRepository,
            IOrderPositionRepository orderPositionRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.orderRepository = orderRepository;
            this.userRolesRepository = userRolesRepository;
            this.orderPositionRepository = orderPositionRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View(productRepository.Products.ToList());
        }

        public IActionResult Categories()
        {
            CategoriesListViewModel vm = new CategoriesListViewModel
            {
                Categories = categoryRepository.Categories.ToList(),
                ProductsInCategory = new Dictionary<string, int>()
            };
            foreach(var category in vm.Categories)
            {
                vm.ProductsInCategory.Add(category.Id, productRepository.Products.Where(x => x.Category.Id == category.Id).Count());
            }
            return View(vm);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Roles()
        {
            RolesListViewModel vm = new RolesListViewModel
            {
                Roles = roleManager.Roles.ToList(),
                UsersInRole = new Dictionary<string, int>()
            };
            foreach(var role in vm.Roles)
            {
                vm.UsersInRole.Add(role.Id, userRolesRepository.UserRoles.Where(x => x.RoleId == role.Id).Count());
            }
            return View(vm);
        }


        public IActionResult Users()
        {
            return View(userManager.Users.ToList());
        }

        public IActionResult Orders()
        {
            return View(orderRepository.Orders.ToList());
        }

        public ActionResult OrderDetails(string id)
        {
            OrderDetailsViewModel vm = new OrderDetailsViewModel
            {
                Order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == id),
                OrderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(x => x.OrderId == id).ToList()
            };
            return View(vm);
        }
    }
}