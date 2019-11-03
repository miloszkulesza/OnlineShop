using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public bool AnySearchingProductExists { get; set; } = false;
        public string SearchText { get; set; }
    }
}
