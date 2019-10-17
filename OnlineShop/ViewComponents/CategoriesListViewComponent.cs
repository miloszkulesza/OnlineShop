using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewComponents
{
    public class CategoriesListViewComponent : ViewComponent
    {
        private ICategoryRepository repo;

        public CategoriesListViewComponent(ICategoryRepository repo)
        {
            this.repo = repo;
        }

        public IViewComponentResult Invoke() => View(repo.Categories.OrderBy(c => c.Name).ToList());
    }
}
