using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Data
{
    public class OrderPositionRepository : IOrderPositionRepository
    {
        private ApplicationDbContext context;

        public OrderPositionRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<OrderPosition> OrderPositions => context.OrderPositions;

        public void SaveOrderPosition(OrderPosition orderPosition)
        {
            if (orderPosition.OrderPositionId == null)
            {
                context.OrderPositions.Add(orderPosition);
            }
            else
            {
                OrderPosition dbEntry = context.OrderPositions.FirstOrDefault(p => p.OrderPositionId == orderPosition.OrderPositionId);
                if (dbEntry != null)
                {
                    dbEntry.OrderId = orderPosition.OrderId;
                    dbEntry.ProductName = orderPosition.ProductName;
                    dbEntry.PurchasePrice = orderPosition.PurchasePrice;
                    dbEntry.Quantity = orderPosition.Quantity;
                }
            }
            context.SaveChanges();
        }
    }
}
