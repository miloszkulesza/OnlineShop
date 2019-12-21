using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace OnlineShop.Infrastructure
{
    public static class UrlExtensions
    {
        public static string ImagePath(this IUrlHelper helper, string nameOfImage)
        {
            var path = Path.Combine("~/images/products/", nameOfImage);
            var absolutePath = helper.Content(path);
            return absolutePath;
        }

        public static string IconPath(this IUrlHelper helper, string nameOfImage)
        {
            var path = Path.Combine("~/images/icons/", nameOfImage);
            var absolutePath = helper.Content(path);
            return absolutePath;
        }

        public static string PathAndQuery(this HttpRequest request) => request.QueryString.HasValue ? $"{request.Path}{request.QueryString}" : request.Path.ToString();

        public static string SaveProductImagePath(this IUrlHelper helper, string fileName)
        {
            var path = Path.Combine("\\wwwroot\\images\\products", fileName);
            var absolutePath = helper.Content(path);
            return absolutePath;
        }
    }
}
