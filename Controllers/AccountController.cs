using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FridgeManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // ✅ GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // ✅ POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            // Get roles
            var roles = await _userManager.GetRolesAsync(user);

            // Admin
            if (roles.Contains(Roles.Admin))
            {
                return RedirectToAction("Dashboard", "Admin", new { area = "Administrator" });
            }

            // Customer
            if (roles.Contains(Roles.Customer))
            {
                return RedirectToAction("Dashboard", "Customer");
            }

            // Employee
            if (roles.Contains(Roles.Employee))
            {
                return user.EmployeeRole switch
                {
                    EmployeeRoles.CustomerManager => RedirectToAction("Index", "CustomerManagementHome", new { area = "CustomerManagementSubSystem" }),
                    EmployeeRoles.FaultTechnician => RedirectToAction("Index", "FaultTech", new { area = "FaultTechSubSystem" }),
                    EmployeeRoles.MaintenanceTechnician => RedirectToAction("Index", "MaintenanceHome", new { area = "MaintenanceSubSystem" }),
                    EmployeeRoles.PurchasingManager => RedirectToAction("Index", "Purchasing", new { area = "PurchasingSubSystem" }),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            // Default fallback
            return RedirectToAction("Index", "Home");
        }

        // ✅ GET: Register Customer
        public IActionResult RegisterCustomer()
        {
            return View();
        }

        // ✅ POST: Register Customer
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                UserType = Roles.Customer
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Ensure role exists
                if (!await _roleManager.RoleExistsAsync(Roles.Customer))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>(Roles.Customer));
                }

                await _userManager.AddToRoleAsync(user, Roles.Customer);

                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // ✅ Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }

        // ✅ Access Denied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
