using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Data
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Product> Products => context.Products;

        public void SaveProduct(Product product)
        {
            if (product.Id == null)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.FirstOrDefault(p => p.Id == product.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                    dbEntry.Quantity = product.Quantity;
                    dbEntry.DateOfAddition = product.DateOfAddition;
                    dbEntry.Id = product.Id;
                    dbEntry.ImageName = product.ImageName;
                    dbEntry.IsHidden = product.IsHidden;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(string productId)
        {
            Product dbEntry = context.Products.FirstOrDefault(p => p.Id == productId);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
