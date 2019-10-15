using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Pole email jest wymagane")]
        [UIHint("email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Pole hasło jest wymagane")]
        [UIHint("password")]
        public string Password { get; set; }
    }
}
