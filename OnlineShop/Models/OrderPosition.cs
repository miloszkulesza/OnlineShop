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
        public string OrderPositionId { get; set; }
        public string OrderId { get; set; } 
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
