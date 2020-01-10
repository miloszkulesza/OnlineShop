using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "Podaj imię")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Podaj nazwisko")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Podaj ulicę")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Podaj nr budynku")]
        [Display(Name = "Numer budynku")]
        public string BuildingNumber { get; set; }

        [Display(Name = "Numer lokalu")]
        public string ApartmentNumber { get; set; }

        [Required(ErrorMessage = "Podaj miasto")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Podaj kod pocztowy")]
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Podaj nr telefonu")]
        [Display(Name = "Telefon")]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Nieprawidłowy format numeru telefonu")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Podaj email")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Potwierdź hasło")]
        public string ConfirmPassword { get; set; }
    }
}
