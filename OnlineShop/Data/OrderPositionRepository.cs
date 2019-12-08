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

        public void SaveOrderPosition(params OrderPosition[] orderPositions)
        {
            foreach (var position in orderPositions)
            {
                if (position.OrderPositionId == null)
                {
                    context.OrderPositions.Add(position);
                }
                else
                {
                    OrderPosition dbEntry = context.OrderPositions.FirstOrDefault(p => p.OrderPositionId == position.OrderPositionId);
                    if (dbEntry != null)
                    {
                        dbEntry.OrderId = position.OrderId;
                        dbEntry.PurchasePrice = position.PurchasePrice;
                        dbEntry.Quantity = position.Quantity;
                    }
                }
                context.SaveChanges();
            }
        }

        public OrderPosition DeleteOrderPosition(OrderPosition orderPosition)
        {
            OrderPosition dbEntry = context.OrderPositions.FirstOrDefault(p => p.OrderPositionId == orderPosition.OrderPositionId);
            if (dbEntry != null)
            {
                context.OrderPositions.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
