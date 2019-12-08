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
            return View(orderRepository.Orders.OrderByDescending(x => x.DateOfAddition).ToList());
        }

        public IActionResult OrderDetails(string id)
        {
            OrderDetailsViewModel vm = new OrderDetailsViewModel
            {
                Order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == id),
                OrderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(x => x.OrderId == id).ToList()
            };
            return View(vm);
        }

        public IActionResult CancelOrder(string id)
        {
            var order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == id);
            var orderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(x => x.OrderId == id).ToList();
            foreach (var orderPosition in orderPositions)
            {
                var product = productRepository.Products.FirstOrDefault(x => x.Name == orderPosition.Product.Name);
                product.Quantity += orderPosition.Quantity;
                productRepository.SaveProduct(product);
            }
            order.OrderStatus = OrderStatus.Anulowano;
            orderPositionRepository.SaveOrderPosition(orderPositions.ToArray());
            orderRepository.SaveOrder(order);
            TempData["SuccesMessage"] = "Udało się anulować zamówienie";
            return RedirectToAction("Orders");
        }

        public IActionResult EditOrder(string id)
        {
            OrderDetailsViewModel vm = new OrderDetailsViewModel
            {
                Order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == id),
                OrderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(x => x.OrderId == id).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EditOrder(OrderDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                orderRepository.SaveOrder(model.Order);
                TempData["SuccesMessage"] = "Udało się zapisać zmiany";
                return RedirectToAction("Orders");
            }
            TempData["ErrorMessage"] = "Nie udało się zapisać zmian";
            return View(model);
        }

        public IActionResult EditOrderProduct(string id)
        {
            OrderPosition orderPosition = orderPositionRepository.OrderPositions.Include(x => x.Product).FirstOrDefault(o => o.OrderPositionId == id);
            var model = new EditOrderProductViewModel()
            {
                OrderPositionId = id,
                OrderId = orderPosition.OrderId,
                ProductName = orderPosition.Product.Name,
                Quantity = orderPosition.Quantity
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditOrderProduct(EditOrderProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Quantity > 0)
                {
                    var orderPosition = orderPositionRepository.OrderPositions.Include(x => x.Product).FirstOrDefault(x => x.OrderPositionId == viewModel.OrderPositionId);
                    if (viewModel.Quantity <= 0)
                    {
                        orderPositionRepository.DeleteOrderPosition(orderPosition);
                        TempData["SuccessMessage"] = "Udało się usunąć pozycję z zamówienia";
                        return RedirectToAction("Orders");
                    }
                    var orderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(o => o.OrderId == viewModel.OrderId).ToList();
                    var product = productRepository.Products.FirstOrDefault(o => o.Name == viewModel.ProductName);
                    product.Quantity += orderPosition.Quantity;
                    if (viewModel.Quantity > product.Quantity)
                    {
                        TempData["ErrorMessage"] = "Nie ma aż tyle tego produktu w magazynie!";
                        return View(viewModel);
                    }
                    orderPosition.Quantity = viewModel.Quantity;
                    product.Quantity -= viewModel.Quantity;
                    productRepository.SaveProduct(product);
                    orderPositionRepository.SaveOrderPosition(orderPosition);
                    var order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == viewModel.OrderId);
                    order.OrderValue = 0;
                    foreach (var element in order.OrderPosition)
                    {
                        order.OrderValue += element.Quantity * element.PurchasePrice;
                    }
                    orderRepository.SaveOrder(order);
                    TempData["SuccessMessage"] = "Udało się zapisać zmiany";
                    return RedirectToAction("Orders");
                }
                else
                {
                    TempData["ErrorMessage"] = "Ilość produktu w zamówieniu nie może być mniejsza od 0.";
                    return View(viewModel);
                }
            }
            return View(viewModel);
        }

        public IActionResult DeleteOrderPosition(string id)
        {
            var orderPosition = orderPositionRepository.OrderPositions.Include(x => x.Product).FirstOrDefault(x => x.OrderPositionId == id);
            var product = productRepository.Products.FirstOrDefault(o => o.Name == orderPosition.Product.Name);
            product.Quantity += orderPosition.Quantity;
            productRepository.SaveProduct(product);
            orderPositionRepository.DeleteOrderPosition(orderPosition);
            var order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == orderPosition.OrderId);
            var orderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(o => o.OrderId == orderPosition.OrderId).ToList();
            if (orderPositions.Count() > 0)
            {
                order.OrderValue = 0;
                foreach (var element in order.OrderPosition)
                {
                    order.OrderValue += element.Quantity * element.PurchasePrice;
                }
                if (order.OrderValue <= 0)
                {
                    order.OrderValue = 0;
                    orderRepository.DeleteOrder(order);
                }
                else
                {
                    orderRepository.SaveOrder(order);
                }
            }
            else
            {
                order.OrderValue = 0;
                order.OrderStatus = OrderStatus.Anulowano;
                orderRepository.SaveOrder(order);
                TempData["SuccessMessage"] = "Anulowano zamówienie";
                return RedirectToAction("Orders");
            }
            TempData["SuccessMessage"] = "Udało sie zapisać zmiany";
            return RedirectToAction("EditOrder", new { id = order.OrderId });
        }

        public IActionResult DeleteCategory(string id)
        {
            var category = categoryRepository.Categories.FirstOrDefault(x => x.Id == id);
            if(category != null)
            {
                if(productRepository.Products.Include(x => x.Category).Where(x => x.Category.Id == category.Id).Count() > 0)
                {
                    TempData["ErrorMessage"] = "Nie można usunąć kategorii w której są produkty";
                    return RedirectToAction("Categories");
                }
                categoryRepository.DeleteCategory(category);
                TempData["SuccessMessage"] = "Udało się usunąć kategorię";
                return RedirectToAction("Categories");
            }
            TempData["ErrorMessage"] = "Kategoria nie istnieje";
            return RedirectToAction("Categories");
        }
    }
}