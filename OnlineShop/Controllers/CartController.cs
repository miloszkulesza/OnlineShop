using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private Cart cart;

        public CartController(IProductRepository repository, Cart cartService)
        {
            this.repository = repository;
            cart = cartService;
        }

        public IActionResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public IActionResult AddToCart(string productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public IActionResult AddOneToCart(int cartLineId)
        {
            var line = cart.Lines.FirstOrDefault(x => x.CartLineID == cartLineId);
            if (line != null)
            {
                if (line.Quantity < line.Product.Quantity)
                {
                    cart.AddItem(line.Product, 1);
                }
                else
                    TempData["ErrorMessage"] = "Masz już maksymalną dostępną ilość tego produktu";
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(string productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public IActionResult DeleteOneFromCart(int cartLineId)
        {
            if(cart.Lines.FirstOrDefault(x => x.CartLineID == cartLineId).Quantity == 1)
            {
                return RedirectToAction("RemoveFromCart", new { productId = cart.Lines.FirstOrDefault(x => x.CartLineID == cartLineId).Product.Id });
            }
            else
            {
                cart.DeleteOneFromCart(cartLineId);
                return RedirectToAction("Index");
            }
        }
    }
}