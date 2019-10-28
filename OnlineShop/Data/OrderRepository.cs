using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Data
{
    public class OrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Order> Orders => context.Orders;

        public Order SaveOrder(Order order)
        {
            if (order.OrderId == null)
            {
                order.OrderId = Guid.NewGuid().ToString();
                context.Orders.Add(order);
                context.SaveChanges();
                return order;
            }
            else
            {
                Order dbEntry = context.Orders.FirstOrDefault(p => p.OrderId == order.OrderId);
                if (dbEntry != null)
                {
                    dbEntry.ApartmentNumber = order.ApartmentNumber;
                    dbEntry.BuildingNumber = order.BuildingNumber;
                    dbEntry.City = order.City;
                    dbEntry.Comment = order.Comment;
                    dbEntry.DateOfAddition = order.DateOfAddition;
                    dbEntry.Email = order.Email;
                    dbEntry.FirstName = order.FirstName;
                    dbEntry.LastName = order.LastName;
                    dbEntry.OrderId = order.OrderId;
                    dbEntry.OrderStatus = order.OrderStatus;
                    dbEntry.OrderValue = order.OrderValue;
                    dbEntry.PhoneNumber = order.PhoneNumber;
                    dbEntry.PostalCode = order.PostalCode;
                    dbEntry.Street = order.Street;
                    dbEntry.UserId = order.UserId;
                }
                context.SaveChanges();
                return dbEntry;
            }
        }
    }
}
