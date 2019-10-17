using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public string ReturnUrl { get; set; }
    }
}
