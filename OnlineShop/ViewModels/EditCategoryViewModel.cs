using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class EditCategoryViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Podaj nazwę kategorii")]
        public string Name { get; set; }
    }
}
