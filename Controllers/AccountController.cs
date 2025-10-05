using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FridgeManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly FridgeDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
             UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager,
             RoleManager<IdentityRole<int>> roleManager,
             FridgeDbContext context,
             ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }


        // ✅ GET: Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1️⃣ Find the user
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            // 2️⃣ Check if customer is verified
            if (await _userManager.IsInRoleAsync(user, Roles.Customer))
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

                if (customer != null && !customer.IsVerified)
                {
                    ModelState.AddModelError("", "Your account is awaiting admin approval.");
                    return View(model);
                }
            }

            // 3️⃣ Sign in
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            // 4️⃣ Get roles
            var roles = await _userManager.GetRolesAsync(user);

            // 5️⃣ Redirect based on role
            if (roles.Contains(Roles.Admin))
                return RedirectToAction("Dashboard", "ManageEmployee", new { area = "Administrator" });

            if (roles.Contains(Roles.Customer))
                return RedirectToAction("Dashboard", "Customer");

            if (roles.Contains(Roles.Employee))
            {
                if (string.IsNullOrEmpty(user.EmployeeRole))
                    return RedirectToAction("Index", "Home");

                return user.EmployeeRole switch
                {
                    EmployeeRoles.CustomerManager => Redirect("~/CustomerManagementSubsystem/CustomerManagementHome"),
                    EmployeeRoles.FaultTechnician => Redirect("~/FaultTechSubsystem/Faults"),
                    EmployeeRoles.MaintenanceTechnician => Redirect("~/MaintenanceSubSystem/MaintenanceHome"),
                    EmployeeRoles.PurchasingManager => Redirect("~/PurchasingSubsystem/Purchasing"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            // 6️⃣ Default fallback
            return RedirectToAction("Index", "Home");
        }


        // ✅ GET: Register Customer
        public IActionResult RegisterCustomer()
        {
            ViewBag.Locations = _context.Locations
                .Where(l => l.IsActive)
                .Select(l => new SelectListItem
                {
                    Value = l.LocationId.ToString(),
                    Text = l.Address
                 }).ToList();
            return View();
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Locations = _context.Locations
                    .Where(l => l.IsActive)
                    .Select(l => new SelectListItem
                    {
                        Value = l.LocationId.ToString(),
                        Text = l.Address
                    }).ToList();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                UserType = Roles.Customer
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(Roles.Customer))
                    await _roleManager.CreateAsync(new IdentityRole<int>(Roles.Customer));

                await _userManager.AddToRoleAsync(user, Roles.Customer);

                var customer = new Customer
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    LocationId = model.LocationId,
                    ApplicationUserId = user.Id,
                    RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                    IsActive = true,
                    IsVerified = false
                };

                _context.Customers.Add(customer);

                // Optional: Admin notification
                _context.AdminNotifications.Add(new AdminNotification
                {
                    Message = $"New customer {customer.FullName} registered. Awaiting verification."
                });

                await _context.SaveChangesAsync();

                TempData["Message"] = "Registration successful. Wait for admin approval.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
        // ✅ Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        // ✅ Access Denied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}