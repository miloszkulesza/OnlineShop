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
    public class CatalogController : Controller
    {
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;

        public CatalogController(IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index(string categoryId)
        {
            ProductsListViewModel productsViewModel;
            if (categoryId == null)
            {
                productsViewModel = new ProductsListViewModel
                {
                    Products = productRepository.Products.OrderBy(p => p.DateOfAddition).ToList()
                };
                return View(productsViewModel);
            }
            productsViewModel = new ProductsListViewModel
            {
                Products = productRepository.Products.Where(p => p.Category.Id == categoryId).OrderBy(p => p.DateOfAddition).ToList(),
                Category = categoryRepository.Categories.FirstOrDefault(c => c.Id == categoryId)
            };
            return View(productsViewModel);
        }
    }
}