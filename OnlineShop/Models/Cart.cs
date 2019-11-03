using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        private static int cartLineId = 0;

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection.FirstOrDefault(p => p.Product.Id == product.Id);
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    CartLineID = NewCartLineId(),
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) => lineCollection.RemoveAll(l => l.Product.Id == product.Id);

        public virtual void DeleteOneFromCart(int cartLineId)
        {
            CartLine line = lineCollection.FirstOrDefault(p => p.CartLineID == cartLineId);
            if (line != null)
            {
                if (line.Quantity > 1)
                {
                    line.Quantity -= 1;
                }
                else
                {
                    RemoveLine(line.Product);
                }
            }
        }

        public virtual decimal ComputeTotalValue() => lineCollection.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;

        public virtual int TotalQuantity() => lineCollection.Sum(l => l.Quantity);

        private static int NewCartLineId()
        {
            return cartLineId++;
        }
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
