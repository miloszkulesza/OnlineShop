using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Podaj imie")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Podaj nazwisko")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Podaj ulicę")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Podaj numer budynku")]
        [Display(Name = "Nr budynku")]
        public string BuildingNumber { get; set; }
        [Display(Name = "Nr lokalu")]
        public string ApartmentNumber { get; set; }
        [Required(ErrorMessage = "Podaj miasto")]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Required(ErrorMessage = "Podaj kod pocztowy")]
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Podaj numer telefonu")]
        [RegularExpression(@"(\+\d{2})*[\d\s-]+", ErrorMessage = "Zły format numeru telefonu")]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Podaj hasło")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Wpisz ponownie hasło")]
        [Display(Name = "Potwierdź hasło")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Podaj email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string ReturnUrl { get; set; }
    }
}
