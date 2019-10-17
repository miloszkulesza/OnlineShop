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

        public static int PageSize { get; set; } = 10;

        public CatalogController(IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index(string categoryId, int productPage = 1)
        {
            ProductsListViewModel productsViewModel;
            if (categoryId == null)
            {
                productsViewModel = new ProductsListViewModel
                {
                    Products = productRepository.Products.OrderBy(p => p.DateOfAddition).Skip((productPage - 1) * PageSize).Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = productRepository.Products.Count()
                    }
                };
                return View(productsViewModel);
            }
            productsViewModel = new ProductsListViewModel
            {
                Products = productRepository.Products.Where(x => x.Category.Id == categoryId).OrderBy(p => p.DateOfAddition).Skip((productPage - 1) * PageSize).Take(PageSize),
                Category = categoryRepository.Categories.FirstOrDefault(c => c.Id == categoryId),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = productRepository.Products.Where(p => p.Category.Id == categoryId).Count()
                }
            };
            return View(productsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetPageSize(ProductsListViewModel model)
        {
            PageSize = model.PagingInfo.ItemsPerPage;
            if (model.Category == null)
                return RedirectToAction("Index", new { productPage = 1 });
            return RedirectToAction("Index", new { categoryId = model.Category.Id, productPage = 1 });
        }
    }
}