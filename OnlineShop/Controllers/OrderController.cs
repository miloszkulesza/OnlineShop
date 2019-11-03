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
    [Authorize]
    public class OrderController : Controller
    {
        private IProductRepository productRepository;
        private Cart cartService;
        private UserManager<AppUser> userManager;
        private IOrderRepository orderRepository;
        private IOrderPositionRepository orderPositionRepository;

        public OrderController(IProductRepository productRepository,
            Cart cartService,
            UserManager<AppUser> userManager,
            IOrderRepository orderRepository,
            IOrderPositionRepository orderPositionRepository)
        {
            this.productRepository = productRepository;
            this.cartService = cartService;
            this.userManager = userManager;
            this.orderPositionRepository = orderPositionRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<IActionResult> CreateOrder()
        {
            if(cartService.Lines.Count() == 0)
            {
                TempData["ErrorMessage"] = "Brak produktów w koszyku";
                return RedirectToAction("Index", new { controller = "Cart" });
            }
            var user = await userManager.GetUserAsync(User);
            var order = new CreateOrderViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Street = user.Street,
                BuildingNumber = user.BuildingNumber,
                ApartmentNumber = user.ApartmentNumber,
                City = user.City,
                PostalCode = user.ZipCode,
                PhoneNumber = user.PhoneNumber
            };
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                var newOrder = new Order
                {
                    DateOfAddition = DateTime.Now,
                    UserId = user.Id,
                    OrderStatus = OrderStatus.Nowy,
                    OrderValue = cartService.ComputeTotalValue(),
                    ApartmentNumber = order.ApartmentNumber,
                    BuildingNumber = order.BuildingNumber,
                    City = order.City,
                    Comment = order.Comment,
                    Email = order.Email,
                    FirstName = order.FirstName,
                    LastName = order.LastName,
                    PhoneNumber = order.PhoneNumber,
                    PostalCode = order.PostalCode,
                    Street = order.Street
                };
                newOrder = orderRepository.SaveOrder(newOrder);
                if (newOrder.OrderPosition == null)
                    newOrder.OrderPosition = new List<OrderPosition>();
                foreach(var line in cartService.Lines)
                {
                    var newOrderPosition = new OrderPosition()
                    {
                        Product = line.Product,
                        Quantity = line.Quantity,
                        PurchasePrice = line.Product.Price * line.Quantity,
                        OrderId = newOrder.OrderId
                    };
                    var product = productRepository.Products.FirstOrDefault(p => p.Id == line.Product.Id);
                    product.Quantity -= line.Quantity;
                    productRepository.SaveProduct(product);
                    newOrder.OrderPosition.Add(newOrderPosition);
                    orderPositionRepository.SaveOrderPosition(newOrderPosition);
                }
                TempData["SuccessMessage"] = "Udało się złożyć zamówienie. Dziękujemy.";
                cartService.Clear();
                return RedirectToAction("Index", new { controller = "Cart" });
            }
            else
                return View(order);
        }

        public async Task<IActionResult> History()
        {
            var user = await userManager.GetUserAsync(User);
            var orders = orderRepository.Orders.Where(x => x.UserId == user.Id).OrderByDescending(x => x.DateOfAddition).ToList();
            return View(orders);
        }

        public IActionResult OrderDetails(string orderId)
        {
            var vm = new OrderDetailsViewModel
            {
                Order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == orderId),
                OrderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(x => x.OrderId == orderId).ToList()
            };
            return View(vm);
        }
    }
}