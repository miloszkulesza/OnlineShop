using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class CategoriesListViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public Dictionary<string, int> ProductsInCategory { get; set; }
    }
}
