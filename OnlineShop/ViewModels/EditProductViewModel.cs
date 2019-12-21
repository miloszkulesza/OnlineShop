using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.ViewModels
{
    public class EditProductViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Podaj nazwę")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Podaj opis")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Podaj cenę")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Podaj ilość")]
        public int Quantity { get; set; }
        public IFormFile ProductImage { get; set; }
        public DateTime DateOfAddition { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public string SelectedCategory { get; set; }
        public bool IsHidden { get; set; } = false;
    }
}
