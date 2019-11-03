using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers
{
    public class SearchController : Controller
    {
        private IProductRepository productRepository;

        public SearchController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string id)
        {
            if (id == null || id.Equals(""))
            {
                return View();
            }
            SearchViewModel searchViewModel = new SearchViewModel();

            searchViewModel.SearchText = id;
            searchViewModel.Products = productRepository.Products.Where(product => product.Name.ToLower().Contains(id.ToLower()));

            if (searchViewModel.Products.Any())
                searchViewModel.AnySearchingProductExists = true;
            else searchViewModel.AnySearchingProductExists = false;

            return View(searchViewModel);
        }
    }
}