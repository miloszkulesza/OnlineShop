using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class OrderPosition
    {
        [Key]
        public string OrderPositionId { get; set; } = Guid.NewGuid().ToString();
        public string OrderId { get; set; } 
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal PurchasePrice { get; set; }
        public virtual Order Order { get; set; }
    }
}
