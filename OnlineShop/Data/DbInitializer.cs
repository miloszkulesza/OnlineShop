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
                        Name = "Motorola One 4/64GB Dual SIM",
                        Category = context.Categories.FirstOrDefault(c => c.Name == "Smartfony"),
                        Quantity = 20,
                        Description = "Motorola One 64 GB czarny ma podwójny aparat, 8-rdzeniowy procesor, kapitalny wyświetlacz Max Vision, a do tego zawsze aktualny system zabezpieczeń. To efekt połączenia sił firm Motorola i Google, czego efektem jest gustowny, szybki, pełen inteligentnych funkcji smartfon. Z nim na nowo odkryjesz muzykę, filmy, gry oraz aplikacje.",
                        Price = 849,
                        DateOfAddition = DateTime.Now,
                        ImageName = "motorola-one-1.png"
                    },
                    new Product
                    {
                        Name = "Huawei P20 Dual SIM 64GB",
                        Category = context.Categories.FirstOrDefault(c => c.Name == "Smartfony"),
                        Quantity = 15,
                        Description = "Ogromna moc i wyrafinowany design purpurowego Huawei P20 64 GB idealnie współgrają ze sobą, tworząc prawdziwe dzieło sztuki. Inteligentne funkcje przenikają każdy aspekt smartfona. Od wydajności, przez czas pracy na baterii, aż po funkcje aparatu, gdzie dają z siebie najwięcej. Dzięki nim każde ujęcie będzie perfekcyjne. Pokaże więcej detali oraz pełnię kolorów. Efekty podziwiać będziesz na ekranie FullView 2.0, który imponuje jakością projekcji.",
                        Price = 1349,
                        DateOfAddition = DateTime.Now,
                        ImageName = "huawei-p20-1.jpg"
                    },
                    new Product
                    {
                        Name = "OnePlus 6T 8/256GB Dual SIM",
                        Category = context.Categories.FirstOrDefault(c => c.Name == "Smartfony"),
                        Quantity = 6,
                        Description = "Oto OnePlus 6T 256 GB Midnight Black – smartfon z genialnym, panoramicznym ekranem 6,41”, matrycą AMOLED oraz zredukowanym do minimum notchem. Jego podwójny aparat doskonale radzi sobie w nocy. Z myślą o fotografii nocnej został zresztą skonstruowany. OnePlus 6T ma ponadto ogromną moc, płynącą z potężnych podzespołów. Jest również intuicyjny w obsłudze, dzięki OxygenOS. A jego design to czysta poezja i kwintesencja elegancji.",
                        Price = 2099,
                        DateOfAddition = DateTime.Now,
                        ImageName = "oneplus-6t-1.jpg"
                    },
                    new Product
                    {
                        Name = "ASUS ZenFone 4 ZE554KL 4",
                        Category = context.Categories.FirstOrDefault(c => c.Name == "Smartfony"),
                        Quantity = 18,
                        Description = "Dzięki licznym udoskonaleniom biały ASUS ZenFone 4 udanie rozwija prestiżowy design serii ZenFone, oferując wyjątkowe piękno oraz nieporównywalne wrażenia podczas użytkowania. Zamknięty w jednolitej obudowie z aluminium, stworzonej przy wykorzystaniu zaawansowanej technologii, ZenFone 4 jest smukły i lekki, ale też niewiarygodnie mocny. Duży, 5,5-calowy wyświetlacz jest otoczony bardzo wąską ramką, co zapewnia wspaniały, niemalże kinowy obraz, a niewielka szerokość telefonu umożliwia wygodną obsługę za pomocą jednej ręki. Podwójny aparat ZenFone 4 jest całkowicie wbudowany w obudowę telefonu dla uzyskania idealnie płaskiej i gładkiej powierzchni tylnej. Zarówno przednia, jak i tylna część telefonu ZenFone 4 są pokryte zaokrąglonym szkłem typu 2.5D Corning® Gorilla® Glass, co zapewnia bardziej wygodny chwyt oraz trwałość i odporność na zarysowania.",
                        Price = 699,
                        DateOfAddition = DateTime.Now,
                        ImageName = "asus-zenfone-1.jpg"
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
                        Name = "ASUS TUF Gaming FX505DU-AL079T",
                        Category = context.Categories.FirstOrDefault(c => c.Name == "Laptopy"),
                        Description = "Gotuj się do walki. Gamingowy laptop ASUS TUF Gaming FX705DU wprowadzi Cię na pola wirtualnych bitew, oddając do dyspozycji arsenał, który poprowadzi Cię do niezliczonych zwycięstw. Wyposażony został w wyselekcjonowane, ultrawydajne komponenty, m.in. w procesor AMD Ryzen 7 oraz kartę graficzną GeForce GTX. Z takim zapleczem technologicznym Twoi rywale mogą co najwyżej przygotowywać się do odwrotu.",
                        Price = 4499,
                        Quantity = 1,
                        DateOfAddition = DateTime.Now,
                        ImageName = "asus-tuf-fx705du-1.jpg"
                    },
                    new Product
                    {
                        Name = "Lenovo Legion Y740-17",
                        Category = context.Categories.FirstOrDefault(c => c.Name == "Laptopy"),
                        Description = "Gotuj się do walki. Gamingowy laptop Lenovo Legion Y740-17 wprowadzi Cię na pola wirtualnych bitew, oddając do dyspozycji arsenał, który poprowadzi Cię do niezliczonych zwycięstw. Wyposażony został w wyselekcjonowane, ultrawydajne komponenty, m.in. w procesor Intel Core i7 9. generacji oraz kartę graficzną GeForce RTX 2070 Max-Q. Z takim zapleczem technologicznym Twoi rywale mogą co najwyżej przygotowywać się do odwrotu.",
                        Price = 8999,
                        Quantity = 3,
                        DateOfAddition = DateTime.Now,
                        ImageName = "lenovo-y740-1.jpg"
                    },
                    new Product
                    {
                        Name = "Dell Inspiron G5 i7-9750H",
                        Category = context.Categories.FirstOrDefault(c => c.Name == "Laptopy"),
                        Description = "Płynna, komfortowa rozgrywka bez lagów i opóźnień to rzecz niezbędna, jeśli w ferworze rywalizacji gamingowej nie chcesz stać się mięsem armatnim. Z Dell Inspiron G5 nie zginiesz, bo to laptop stworzony, by zwyciężać. Za wydajność odpowiadają procesor Intel Core i7 w duecie z kartą graficzną NVIDIA GeForce RTX. Twoim oknem na pole bitwy jest natomiast ekran z częstotliwością odświeżania 144 Hz, za którym maluje się płynny, wyrazisty obraz.",
                        Price = 6799,
                        Quantity = 1,
                        DateOfAddition = DateTime.Now,
                        ImageName = "dell-g5-i7-1.jpg"
                    },
                    new Product
                    {
                        Name = "ASUS VivoBook S530FA",
                        Category = context.Categories.FirstOrDefault(c => c.Name == "Laptopy"),
                        Description = "VivoBook S15 wniesie nieco więcej charakteru do Twojego aktywnego stylu życia, oferując nowe, odważne wzornictwo, bardzo lekką konstrukcję, a także nowatorski ekran NanoEdge z trzema wąskimi ramkami i zawiasem ErgoLift.",
                        Price = 2899,
                        Quantity = 10,
                        DateOfAddition = DateTime.Now,
                        ImageName = "asus-vivobook-s15-1.jpg"
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
