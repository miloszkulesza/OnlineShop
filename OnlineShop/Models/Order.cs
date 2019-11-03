using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public DateTime DateOfAddition { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal OrderValue { get; set; }
        public List<OrderPosition> OrderPosition { get; set; }
    }

    public enum OrderStatus
    {
        [Display(Name = "Nowy")]
        Nowy,
        [Display(Name = "Przyjęto do realizacji")]
        Przyjeto_do_realizacji,
        [Display(Name = "Wysłano do klienta")]
        Wyslano_do_klienta,
        [Display(Name = "Zamknięty")]
        Zamkniety,
        [Display(Name = "Anulowano")]
        Anulowano
    }
}
