using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Podaj nowe hasło")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Hasło musi mieć długość minimum {2} znaków", MinimumLength = 8)]
        [Display(Name = "Nowe hasło")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Hasło powinno mieć minimum 8 znaków, jedną wielką literę, jedną małą literę i jeden znak specjalny")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Potwierdź hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są takie same")]
        public string ConfirmPassword { get; set; }
    }
}
