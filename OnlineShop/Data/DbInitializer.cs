using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Data
{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            if(!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Name = "Smartfony"
                    },
                    new Category
                    {
                        Name = "Laptopy"
                    },
                    new Category
                    {
                        Name = "Tablety"
                    }
                );
                context.SaveChanges();
            }
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Xiaomi Mi 9T",
                        Category = context.Categories.FirstOrDefault(c => c.Name == "Smartfony"),
                        Quantity = 5,
                        Description = "Bezramkowe doświadczenie w pełnej krasie! Xiaomi Mi 9T, europejski odpowiednik niezwykle udanego Redmi K20, to prawdziwie immersyjne doznanie podyktowane dużym, pozbawionym notcha ekranem AMOLED oraz wysuwanym aparatem do selfie! Ścisła współpraca szybkiego procesora Snapdragon 730 oraz 6GB pamięci RAM zapewni odpowiednią moc wymaganą do najtrudniejszych zadań, a duża bateria 4000mAh wyniesie komfort użytkowania na zupełnie nowy poziom.",
                        Price = 1599,
                        DateOfAddition = DateTime.Now,
                        ImageName = "mi-9t-1.jpg"
                    },
                    new Product
                    {
                        Name = "HP Pavilion Gaming i5-9300H/16GB/256/Win10x GTX1650",
                        Category = context.Categories.FirstOrDefault(c => c.Name == "Laptopy"),
                        Description = "Gotuj się do walki. Gamingowy laptop HP Pavilion 15 wprowadzi Cię na pola wirtualnych bitew, oddając do dyspozycji arsenał, który poprowadzi Cię do niezliczonych zwycięstw. Wyposażony został w wyselekcjonowane, ultrawydajne komponenty, m.in. w procesor Intel Core i5 9. generacji oraz kartę graficzną GeForce GTX. Z takim zapleczem technologicznym Twoi rywale mogą co najwyżej przygotowywać się do odwrotu.",
                        Price = 4099,
                        Quantity = 2,
                        DateOfAddition = DateTime.Now,
                        ImageName = "hp-pavilion-gaming-1.jpg"
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy TAB S6 10.5 T865 LTE 6/128GB",
                        Category = context.Categories.FirstOrDefault(c => c.Name == "Tablety"),
                        Description = "Odkryj moc komputera zamkniętą w mobilnej formie szarego tabletu Samsung Galaxy Tab S6 LTE. Ultra-smukła, aluminiowa konstrukcja o grubości zaledwie 5,7 mm mieści w sobie moc flagowego procesora Snapdragon 855 oraz bezkompromisową wytrzymałość baterii 7040 mAh. Całość przykrywa ekran 10,5 z bajecznie kolorową matrycą Super AMOLED, którą obsłużysz udoskonalonym rysikiem S-Pen, teraz obsługującym również gesty. Całość uzupełnia rewelacyjny podwójny aparat i nieograniczony dostęp do błyskawicznego internetu za sprawą wbudowanego modemu LTE.",
                        Price = 3449,
                        Quantity = 10,
                        DateOfAddition = DateTime.Now,
                        ImageName = "galaxy-tab-s6-1.jpg"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
