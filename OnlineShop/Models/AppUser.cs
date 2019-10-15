using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "Podaj imie")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Podaj nazwisko")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Podaj ulicę")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Podaj numer budynku")]
        public int BuildingNumber { get; set; }
        [Required(ErrorMessage = "Podaj numer lokalu")]
        public int? ApartmentNumber { get; set; }
        [Required(ErrorMessage = "Podaj miasto")]
        public string City { get; set; }
        [Required(ErrorMessage = "Podaj kod pocztowy")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Podaj numer telefonu")]
        [RegularExpression(@"(\+\d{2})*[\d\s-]+", ErrorMessage = "Zły format numeru telefonu")]
        public override string PhoneNumber { get; set; }
    }
}
