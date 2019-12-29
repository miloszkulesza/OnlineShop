using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IPasswordHasher<AppUser> passwordHasher,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
            this.roleManager = roleManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                        return Redirect(returnUrl ?? "/");
                }
                ModelState.AddModelError(nameof(LoginViewModel.Email), "Nieprawidłowa nazwa użytkownika lub hasło");
            }
            return View(details);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            var vm = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(vm);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Password), "Podane hasła nie są takie same");
                    ModelState.AddModelError(nameof(RegisterViewModel.ConfirmPassword), "Podane hasła nie są takie same");
                    return View(model);
                }
                var user = new AppUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, ApartmentNumber = model.ApartmentNumber,
                    BuildingNumber = model.BuildingNumber, City = model.City, PhoneNumber = model.PhoneNumber, Street = model.Street, ZipCode = model.ZipCode };
                var result = await userManager.CreateAsync(user, model.Password);
                user = await userManager.FindByNameAsync(user.UserName);
                var userRole = await roleManager.FindByNameAsync("Użytkownik");
                var addToRoleResult = await userManager.AddToRoleAsync(user, userRole.NormalizedName);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    if (model.ReturnUrl != null)
                        return Redirect(model.ReturnUrl);
                    TempData["MessageSuccess"] = "Zarejestrowano i zalogowano pomyślnie";
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            return View(model);
        }

        public async Task<IActionResult> Manage(string returnUrl)
        {
            var user = await userManager.GetUserAsync(User);
            var vm = new ManageAccountViewModel
            {
                ChangePassword = new ChangePasswordViewModel(),
                EditAccount = new EditAccountViewModel
                {
                    Email = user.Email,
                    ApartmentNumber = user.ApartmentNumber,
                    BuildingNumber = user.BuildingNumber,
                    City = user.City,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Street = user.Street,
                    ZipCode = user.ZipCode
                },
                ReturnUrl = returnUrl
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(ManageAccountViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                user.ApartmentNumber = model.EditAccount.ApartmentNumber;
                user.BuildingNumber = model.EditAccount.BuildingNumber;
                user.City = model.EditAccount.City;
                user.Email = model.EditAccount.Email;
                user.FirstName = model.EditAccount.FirstName;
                user.LastName = model.EditAccount.LastName;
                user.NormalizedEmail = model.EditAccount.Email;
                user.NormalizedUserName = model.EditAccount.Email;
                user.PhoneNumber = model.EditAccount.PhoneNumber;
                user.Street = model.EditAccount.Street;
                user.UserName = model.EditAccount.Email;
                user.ZipCode = model.EditAccount.ZipCode;
                try
                {
                    var result = await userManager.UpdateAsync(user);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                TempData["SuccessMessage"] = "Udało się zapisać zmiany";
                return View(model);
            }
            TempData["ErrorMessage"] = "Uzupełnij poprawnie formularz zmiany danych";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ManageAccountViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                user.PasswordHash = passwordHasher.HashPassword(user, model.ChangePassword.Password);
                try
                {
                    var result = await userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                model.ChangePassword = new ChangePasswordViewModel();
                TempData["SuccessMessage"] = "Udało się zmienić hasło";
                return View("Manage", model);
            }
            TempData["ErrorMessage"] = "Uzupełnij poprawnie formularz zmiany hasła";
            model.ChangePassword = new ChangePasswordViewModel();
            return View("Manage", model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}