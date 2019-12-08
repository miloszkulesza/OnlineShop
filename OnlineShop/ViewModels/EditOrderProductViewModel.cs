using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class EditOrderProductViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string OrderId { get; set; }
        public string OrderPositionId { get; set; }
    }
}
