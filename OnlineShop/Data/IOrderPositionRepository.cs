using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Data
{
    public interface IOrderPositionRepository
    {
        IQueryable<OrderPosition> OrderPositions { get; }
        void SaveOrderPosition(params OrderPosition[] orderPosition);
    }
}
