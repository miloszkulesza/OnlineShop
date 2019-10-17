using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class CatalogController : Controller
    {
        private IProductRepository productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IActionResult Index(string categoryId)
        {
            List<Product> products;
            if(categoryId == null)
            {
                products = productRepository.Products.OrderBy(p => p.DateOfAddition).ToList();
                return View(products);
            }
            products = productRepository.Products.Where(p => p.Category.Id == categoryId).OrderBy(p => p.DateOfAddition).ToList();
            return View(products);
        }
    }
}