using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class ManageAccountViewModel
    {
        public EditAccountViewModel EditAccount { get; set; }
        public ChangePasswordViewModel ChangePassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}
