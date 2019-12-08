using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class RolesListViewModel
    {
        public IEnumerable<IdentityRole> Roles { get; set; }
        public Dictionary<string, int> UsersInRole { get; set; }
    }
}
