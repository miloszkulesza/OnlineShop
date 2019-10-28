using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Wprowadź imię")]
        [StringLength(50)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Wprowadź nazwisko")]
        [StringLength(50)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Wprowadź ulicę")]
        [StringLength(100)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Wprowadź miasto")]
        [StringLength(100)]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Required(ErrorMessage = "Wprowadź kod pocztowy")]
        [StringLength(6)]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Wprowadź numer budynku")]
        [Display(Name = "Nr budynku")]
        public string BuildingNumber { get; set; }
        [Display(Name = "Nr lokalu")]
        public string ApartmentNumber { get; set; }
        [Required(ErrorMessage = "Podaj numer telefonu")]
        [RegularExpression(@"(\+\d{2})*[\d\s-]+", ErrorMessage = "Zły format numeru telefonu")]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Uwagi do zamówienia")]
        public string Comment { get; set; }
        [Display(Name = "Razem")]
        public decimal OrderValue { get; set; }
    }
}
