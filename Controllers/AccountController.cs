using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;

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

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            // Get roles
            var roles = await _userManager.GetRolesAsync(user);

            // Log roles for debugging
            _logger.LogInformation($"User {user.UserName} roles: {string.Join(", ", roles)}");

            if (roles.Contains(Roles.Admin))
                return RedirectToAction("Dashboard", "ManageEmployee", new { area = "Administrator" });

            if (roles.Contains(Roles.Customer))
                return RedirectToAction("Dashboard", "Customer");

            if (roles.Contains(Roles.Employee))
            {
                if (string.IsNullOrEmpty(user.EmployeeRole))
                {
                    // Fallback if EmployeeRole not set
                    return RedirectToAction("Index", "Home");
                }

                return user.EmployeeRole switch
                {
                    EmployeeRoles.CustomerManager => Redirect("~/CustomerManagementSubsystem/CustomerManagementHome"),
                    EmployeeRoles.FaultTechnician => Redirect("~/FaultTechSubsystem/FaultTech"),
                    EmployeeRoles.MaintenanceTechnician => Redirect("~/MaintenanceSubsystem/MaintenanceHome"),
                    EmployeeRoles.PurchasingManager => Redirect("~/PurchasingSubsystem/Purchasing"),
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
                // ✅ Step 1: Ensure the Customer role exists
                if (!await _roleManager.RoleExistsAsync(Roles.Customer))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>(Roles.Customer));
                }

                // ✅ Step 2: Add user to Customer role
                await _userManager.AddToRoleAsync(user, Roles.Customer);

                // ✅ Step 3: Create matching Customer profile
                var customer = new Customer
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    LocationId = model.LocationId, // only if it's in your viewmodel
                    ApplicationUserId = user.Id,   // link ApplicationUser ↔ Customer
                    RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                    IsActive = true
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                // ✅ Step 4: Redirect to login
                return RedirectToAction("Login", "Account");
            }

            // ❌ If registration failed, show errors
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
            return RedirectToAction("Login", "Account");
        }

        // ✅ Access Denied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}