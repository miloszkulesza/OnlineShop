using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Infrastructure;
using OnlineShop.Models;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Administrator, Pracownik")]
    public class AdminController : Controller
    {
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        private IOrderRepository orderRepository;
        private IUserRolesRepository userRolesRepository;
        private IOrderPositionRepository orderPositionRepository;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;

        public AdminController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            IOrderRepository orderRepository,
            IUserRolesRepository userRolesRepository,
            IOrderPositionRepository orderPositionRepository,
            IPasswordValidator<AppUser> passwordValidator,
            IPasswordHasher<AppUser> passwordHasher)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.orderRepository = orderRepository;
            this.userRolesRepository = userRolesRepository;
            this.orderPositionRepository = orderPositionRepository;
            this.passwordValidator = passwordValidator;
            this.passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            var products = productRepository.Products.ToList();
            return View(products);
        }

        public IActionResult Categories()
        {
            CategoriesListViewModel vm = new CategoriesListViewModel
            {
                Categories = categoryRepository.Categories.ToList(),
                ProductsInCategory = new Dictionary<string, int>()
            };
            foreach(var category in vm.Categories)
            {
                vm.ProductsInCategory.Add(category.Id, productRepository.Products.Where(x => x.Category.Id == category.Id).Count());
            }
            return View(vm);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Roles()
        {
            RolesListViewModel vm = new RolesListViewModel
            {
                Roles = roleManager.Roles.ToList(),
                UsersInRole = new Dictionary<string, int>()
            };
            foreach(var role in vm.Roles)
            {
                vm.UsersInRole.Add(role.Id, userRolesRepository.UserRoles.Where(x => x.RoleId == role.Id).Count());
            }
            return View(vm);
        }


        public IActionResult Users()
        {
            return View(userManager.Users.ToList());
        }

        public IActionResult Orders()
        {
            return View(orderRepository.Orders.OrderByDescending(x => x.DateOfAddition).ToList());
        }

        public IActionResult OrderDetails(string id)
        {
            OrderDetailsViewModel vm = new OrderDetailsViewModel
            {
                Order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == id),
                OrderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(x => x.OrderId == id).ToList()
            };
            return View(vm);
        }

        public IActionResult CancelOrder(string id)
        {
            var order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == id);
            var orderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(x => x.OrderId == id).ToList();
            foreach (var orderPosition in orderPositions)
            {
                var product = productRepository.Products.FirstOrDefault(x => x.Name == orderPosition.Product.Name);
                product.Quantity += orderPosition.Quantity;
                productRepository.SaveProduct(product);
            }
            order.OrderStatus = OrderStatus.Anulowano;
            orderPositionRepository.SaveOrderPosition(orderPositions.ToArray());
            orderRepository.SaveOrder(order);
            TempData["SuccesMessage"] = "Udało się anulować zamówienie";
            return RedirectToAction("Orders");
        }

        public IActionResult EditOrder(string id)
        {
            OrderDetailsViewModel vm = new OrderDetailsViewModel
            {
                Order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == id),
                OrderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(x => x.OrderId == id).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EditOrder(OrderDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                orderRepository.SaveOrder(model.Order);
                TempData["SuccesMessage"] = "Udało się zapisać zmiany";
                return RedirectToAction("Orders");
            }
            TempData["ErrorMessage"] = "Nie udało się zapisać zmian";
            return View(model);
        }

        public IActionResult EditOrderProduct(string id)
        {
            OrderPosition orderPosition = orderPositionRepository.OrderPositions.Include(x => x.Product).FirstOrDefault(o => o.OrderPositionId == id);
            var model = new EditOrderProductViewModel()
            {
                OrderPositionId = id,
                OrderId = orderPosition.OrderId,
                ProductName = orderPosition.Product.Name,
                Quantity = orderPosition.Quantity
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditOrderProduct(EditOrderProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Quantity > 0)
                {
                    var orderPosition = orderPositionRepository.OrderPositions.Include(x => x.Product).FirstOrDefault(x => x.OrderPositionId == viewModel.OrderPositionId);
                    if (viewModel.Quantity <= 0)
                    {
                        orderPositionRepository.DeleteOrderPosition(orderPosition);
                        TempData["SuccessMessage"] = "Udało się usunąć pozycję z zamówienia";
                        return RedirectToAction("Orders");
                    }
                    var orderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(o => o.OrderId == viewModel.OrderId).ToList();
                    var product = productRepository.Products.FirstOrDefault(o => o.Name == viewModel.ProductName);
                    product.Quantity += orderPosition.Quantity;
                    if (viewModel.Quantity > product.Quantity)
                    {
                        TempData["ErrorMessage"] = "Nie ma aż tyle tego produktu w magazynie!";
                        return View(viewModel);
                    }
                    orderPosition.Quantity = viewModel.Quantity;
                    product.Quantity -= viewModel.Quantity;
                    productRepository.SaveProduct(product);
                    orderPositionRepository.SaveOrderPosition(orderPosition);
                    var order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == viewModel.OrderId);
                    order.OrderValue = 0;
                    foreach (var element in order.OrderPosition)
                    {
                        order.OrderValue += element.Quantity * element.PurchasePrice;
                    }
                    orderRepository.SaveOrder(order);
                    TempData["SuccessMessage"] = "Udało się zapisać zmiany";
                    return RedirectToAction("Orders");
                }
                else
                {
                    TempData["ErrorMessage"] = "Ilość produktu w zamówieniu nie może być mniejsza od 0.";
                    return View(viewModel);
                }
            }
            return View(viewModel);
        }

        public IActionResult DeleteOrderPosition(string id)
        {
            var orderPosition = orderPositionRepository.OrderPositions.Include(x => x.Product).FirstOrDefault(x => x.OrderPositionId == id);
            var product = productRepository.Products.FirstOrDefault(o => o.Name == orderPosition.Product.Name);
            product.Quantity += orderPosition.Quantity;
            productRepository.SaveProduct(product);
            orderPositionRepository.DeleteOrderPosition(orderPosition);
            var order = orderRepository.Orders.FirstOrDefault(x => x.OrderId == orderPosition.OrderId);
            var orderPositions = orderPositionRepository.OrderPositions.Include(x => x.Product).Where(o => o.OrderId == orderPosition.OrderId).ToList();
            if (orderPositions.Count() > 0)
            {
                order.OrderValue = 0;
                foreach (var element in order.OrderPosition)
                {
                    order.OrderValue += element.Quantity * element.PurchasePrice;
                }
                if (order.OrderValue <= 0)
                {
                    order.OrderValue = 0;
                    orderRepository.DeleteOrder(order);
                }
                else
                {
                    orderRepository.SaveOrder(order);
                }
            }
            else
            {
                order.OrderValue = 0;
                order.OrderStatus = OrderStatus.Anulowano;
                orderRepository.SaveOrder(order);
                TempData["SuccessMessage"] = "Anulowano zamówienie";
                return RedirectToAction("Orders");
            }
            TempData["SuccessMessage"] = "Udało sie zapisać zmiany";
            return RedirectToAction("EditOrder", new { id = order.OrderId });
        }

        public IActionResult DeleteCategory(string id)
        {
            var category = categoryRepository.Categories.FirstOrDefault(x => x.Id == id);
            if(category != null)
            {
                if(productRepository.Products.Include(x => x.Category).Where(x => x.Category.Id == category.Id).Count() > 0)
                {
                    TempData["ErrorMessage"] = "Nie można usunąć kategorii w której są produkty";
                    return RedirectToAction("Categories");
                }
                categoryRepository.DeleteCategory(category);
                TempData["SuccessMessage"] = "Udało się usunąć kategorię";
                return RedirectToAction("Categories");
            }
            TempData["ErrorMessage"] = "Kategoria nie istnieje";
            return RedirectToAction("Categories");
        }

        public IActionResult HideProduct(string id)
        {
            var product = productRepository.Products.FirstOrDefault(x => x.Id == id);
            if(product != null)
            {
                product.IsHidden = true;
                productRepository.SaveProduct(product);
                TempData["SuccessMessage"] = "Ukryto produkt";
                return RedirectToAction("Products");
            }
            TempData["ErrorMessage"] = "Produkt nie istnieje";
            return RedirectToAction("Products");
        }

        public IActionResult ShowProduct(string id)
        {
            var product = productRepository.Products.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                product.IsHidden = false;
                productRepository.SaveProduct(product);
                TempData["SuccessMessage"] = "Pokazano produkt";
                return RedirectToAction("Products");
            }
            TempData["ErrorMessage"] = "Produkt nie istnieje";
            return RedirectToAction("Products");
        }

        public IActionResult AddProduct(string id)
        {
            EditProductViewModel vm = new EditProductViewModel();
            var categories = categoryRepository.Categories.ToList();
            vm.Categories = new List<SelectListItem>();
            vm.Categories.Add(new SelectListItem("Wybierz kategorię", "none", false));
            foreach (var category in categories)
            {
                vm.Categories.Add(new SelectListItem(category.Name, category.Id));
            }
            if (id != null)
            {
                var product = productRepository.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
                vm.SelectedCategory = product.Category.Id;
                vm.Categories.FirstOrDefault(x => x.Value == product.Category.Id).Selected = true;
                vm.IsHidden = product.IsHidden;
                vm.Id = product.Id;
                vm.Name = product.Name;
                vm.Price = product.Price;
                vm.Quantity = product.Quantity;
                vm.DateOfAddition = product.DateOfAddition;
                vm.Description = product.Description;
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(EditProductViewModel model)
        {
            var categories = categoryRepository.Categories.ToList();
            model.Categories = new List<SelectListItem>();
            model.Categories.Add(new SelectListItem("Wybierz kategorię", "none", true));
            foreach (var category in categories)
            {
                model.Categories.Add(new SelectListItem(category.Name, category.Id));
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if(model.Quantity < 0)
            {
                ModelState.AddModelError("", "Ilość produktu nie może być mniejsza od 0");
                return View(model);
            }
            if (model.Price < 0)
            {
                ModelState.AddModelError("", "Cena produktu nie może być mniejsza od 0");
                return View(model);
            }
            Product newProduct = new Product
            {
                Category = categoryRepository.Categories.FirstOrDefault(x => x.Id == model.SelectedCategory),
                DateOfAddition = DateTime.Now,
                Description = model.Description,
                IsHidden = model.IsHidden,
                Name = model.Name,
                Price = model.Price,
                Quantity = model.Quantity,
            };
            if(model.Id != null)
            {
                newProduct.Id = model.Id;
            }
            if(model.ProductImage == null)
            {
                newProduct.ImageName = productRepository.Products.FirstOrDefault(x => x.Id == model.Id).ImageName;
            }
            else
            {
                var path = $"{Environment.CurrentDirectory}{Url.SaveProductImagePath(model.ProductImage.FileName)}";
                using (var stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    await model.ProductImage.CopyToAsync(stream);
                }
                newProduct.ImageName = model.ProductImage.FileName;
            }    
            productRepository.SaveProduct(newProduct);
            TempData["SuccessMessage"] = "Dodano nowy produkt";
            return RedirectToAction("Products");
        }

        public IActionResult AddCategory(string id)
        {
            EditCategoryViewModel vm = new EditCategoryViewModel();
            if(id != null)
            {
                var category = categoryRepository.Categories.FirstOrDefault(x => x.Id == id);
                vm.Id = category.Id;
                vm.Name = category.Name;
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddCategory(EditCategoryViewModel model)
        {
            if(ModelState.IsValid)
            {
                Category category;
                if(model.Id != null)
                {
                    category = categoryRepository.Categories.FirstOrDefault(x => x.Id == model.Id);
                    category.Name = model.Name;
                }
                else
                {
                    category = new Category
                    {
                        Name = model.Name
                    };
                }
                categoryRepository.SaveCategory(category);
                TempData["SuccessMessage"] = "Zapisano kategorię";
                return RedirectToAction("Categories");
            }
            return View(model);
        }

        public async Task<IActionResult> EditRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            foreach (var user in userManager.Users)
            {
                var isInRole = await userManager.IsInRoleAsync(user, role.NormalizedName);
                if (isInRole)
                    members.Add(user);
                else
                    nonMembers.Add(user);
            }
            return View(new RoleEditViewModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleModificationViewModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            AddErrorsFromResult(result);
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            AddErrorsFromResult(result);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Udało się zapisać użytkowników w grupach";
                return RedirectToAction("Roles");
            }
            else
                return await EditRole(model.RoleId);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            EditUserViewModel vm = new EditUserViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                var user = await userManager.FindByIdAsync(id);
                if(user != null)
                {
                    vm = new EditUserViewModel
                    {
                        ApartmentNumber = user.ApartmentNumber,
                        BuildingNumber = user.BuildingNumber,
                        City = user.City,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Street = user.Street,
                        ZipCode = user.ZipCode
                    };
                }
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(vm.Email);
                if(user == null)
                {
                    user = new AppUser
                    {
                        ApartmentNumber = vm.ApartmentNumber,
                        BuildingNumber = vm.BuildingNumber,
                        City = vm.City,
                        Email = vm.Email,
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        PhoneNumber = vm.PhoneNumber,
                        Street = vm.Street,
                        UserName = vm.Email,
                        ZipCode = vm.ZipCode
                    };
                    if(string.IsNullOrEmpty(vm.Password))
                    {
                        ModelState.AddModelError("", "Nie podano hasła");
                        return View(vm);
                    }
                    var passwordValidateResult = await passwordValidator.ValidateAsync(userManager, user, vm.Password);
                    if(!passwordValidateResult.Succeeded)
                    {
                        AddErrorsFromResult(passwordValidateResult);
                        return View(vm);
                    }
                    if(!vm.Password.Equals(vm.ConfirmPassword))
                    {
                        ModelState.AddModelError("", "Podane hasła nie są takie same");
                        return View(vm);
                    }
                    await userManager.CreateAsync(user, vm.Password);
                    var createdUser = await userManager.FindByNameAsync(vm.Email);
                    await userManager.AddToRoleAsync(createdUser, "Użytkownik");
                    TempData["SuccessMessage"] = "Udało się utworzyć użytkownika";
                    return RedirectToAction("Users");
                }
                else
                {
                    user.ApartmentNumber = vm.ApartmentNumber;
                    user.BuildingNumber = vm.BuildingNumber;
                    user.City = vm.City;
                    user.Email = vm.Email;
                    user.FirstName = vm.FirstName;
                    user.LastName = vm.LastName;
                    user.PhoneNumber = vm.PhoneNumber;
                    user.Street = vm.Street;
                    user.UserName = vm.Email;
                    user.ZipCode = vm.ZipCode;
                    if(!string.IsNullOrEmpty(vm.Password))
                    {                        
                        var passwordValidateResult = await passwordValidator.ValidateAsync(userManager, user, vm.Password);
                        if (!passwordValidateResult.Succeeded)
                        {
                            AddErrorsFromResult(passwordValidateResult);
                            return View(vm);
                        }
                        if (!vm.Password.Equals(vm.ConfirmPassword))
                        {
                            ModelState.AddModelError("", "Podane hasła nie są takie same");
                            return View(vm);
                        }
                        user.PasswordHash = passwordHasher.HashPassword(user, vm.Password);
                    }
                    await userManager.UpdateAsync(user);
                    TempData["SuccessMessage"] = "Udało się edytować użytkownika";
                    return RedirectToAction("Users");
                }
            }
            return View(vm);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}