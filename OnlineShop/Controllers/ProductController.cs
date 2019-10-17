using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IActionResult Index() => View(productRepository.Products.ToList());

        public IActionResult ProductDetails(string productId, string returnUrl) => View(new ProductDetailsViewModel { Product = productRepository.Products.FirstOrDefault(x => x.Id == productId), ReturnUrl = returnUrl });
    }
}