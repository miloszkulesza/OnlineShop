using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Category> Categories => context.Categories;

        public void SaveCategory(Category category)
        {
            if (category.Id == null)
            {
                context.Categories.Add(category);
            }
            else
            {
                Category dbEntry = context.Categories.FirstOrDefault(c => c.Id == category.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = category.Name;
                }
            }
            context.SaveChanges();
        }

        public Category DeleteCategory(Category category)
        {
            Category dbEntry = context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (dbEntry != null)
            {
                context.Categories.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
