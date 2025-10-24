using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly FridgeDbContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly INotificationService _notificationService;

        public AccountController(
             UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager,
             RoleManager<IdentityRole<int>> roleManager,
             FridgeDbContext context,
             ILogger<AccountController> logger,INotificationService notificationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
            _notificationService = notificationService;
        }

        // --------------------
        // Login (GET + POST) — unchanged except we check customer verification
        // --------------------
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            // First verify the password and sign in
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            // Check customer approval
            if (await _userManager.IsInRoleAsync(user, Roles.Customer))
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

                if (customer != null)
                {
                    if (!customer.IsVerified && customer.IsActive)
                    {
                        await _signInManager.SignOutAsync();
                        TempData["LoginError"] = "Your registration is awaiting admin approval.";
                        return RedirectToAction("Login");
                    }

                    if (!customer.IsActive)
                    {
                        await _signInManager.SignOutAsync();
                        TempData["LoginError"] = "Your registration has been rejected. Please contact support.";
                        return RedirectToAction("Login");
                    }
                }
            }
            var roles = await _userManager.GetRolesAsync(user);
            _logger.LogInformation($"User {user.UserName} roles: {string.Join(",", roles)}");

            if (roles.Contains(Roles.Admin))
                return RedirectToAction("Dashboard", "ManageEmployee", new { area = "Administrator" });

            if (roles.Contains(Roles.Customer))
                return RedirectToAction("Dashboard", "Customer");

            if (roles.Contains(Roles.Employee))
            {
                if (string.IsNullOrEmpty(user.EmployeeRole)) return RedirectToAction("Index", "Home");

                return user.EmployeeRole switch
                {
                    EmployeeRoles.CustomerManager => Redirect("~/CustomerManagementSubsystem/CustomerManagementHome"),
                    EmployeeRoles.FaultTechnician => Redirect("~/FaultTechSubsystem/Faults"),
                    EmployeeRoles.MaintenanceTechnician => Redirect("~/MaintenanceSubSystem/MaintenanceHome"),
                    EmployeeRoles.PurchasingManager => RedirectToAction("Index", "Purchase", new { area = "PurchasingSubsystem" }),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            return RedirectToAction("Index", "Home");
        }

        // --------------------
        // Register Customer (GET + POST)
        // --------------------
        [HttpGet]
        public IActionResult RegisterCustomer()
        {
            ViewBag.Locations = _context.Locations
                .Where(l => l.IsActive)
                .Select(l => new SelectListItem { Value = l.LocationId.ToString(), Text = l.Address })
                .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Locations = _context.Locations
                    .Where(l => l.IsActive)
                    .Select(l => new SelectListItem { Value = l.LocationId.ToString(), Text = l.Address })
                    .ToList();

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

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors) ModelState.AddModelError("", err.Description);

                ViewBag.Locations = _context.Locations
                    .Where(l => l.IsActive)
                    .Select(l => new SelectListItem { Value = l.LocationId.ToString(), Text = l.Address })
                    .ToList();

                return View(model);
            }

            if (!await _roleManager.RoleExistsAsync(Roles.Customer))
                await _roleManager.CreateAsync(new IdentityRole<int>(Roles.Customer));
            await _userManager.AddToRoleAsync(user, Roles.Customer);

            // Hash the security answer before saving
            using var sha256 = SHA256.Create();
            var securityHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(model.SecurityAnswer.ToLower().Trim())));

            var customer = new Customer
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                LocationId = model.LocationId,
                ApplicationUserId = user.Id,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                IsActive = true,
                IsVerified = false, // admin must verify
                SecurityQuestion = model.SecurityQuestion,
                SecurityAnswerHash = securityHash
            };

            _context.Customers.Add(customer);

            

            await _context.SaveChangesAsync();
            // Notify the admins
            var adminIds = await _userManager.GetUsersInRoleAsync(Roles.Admin);
            foreach (var admin in adminIds)
            {
                await _notificationService.CreateAsync(
                    admin.Id,
                    $"New customer '{customer.FullName}' is awaiting approval.",
                    "/Administrator/ManageCustomer/Index"
                );
            }
            // ✅ Show success popup BEFORE redirect
            TempData["RegistrationSuccess"] = "Registration successful! Your account is awaiting admin approval.";
            return RedirectToAction("Login");
        }

        // --------------------
        // Logout + AccessDenied
        // --------------------
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();

        // ---------------------------
        // ForgetPassword (enter email) - GET + POST
        // ---------------------------
        [HttpGet]
        public IActionResult ForgetPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (string.IsNullOrEmpty(model?.Email))
            {
                ModelState.AddModelError("", "Email is required.");
                return View(model);
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
            if (customer == null)
            {
                ModelState.AddModelError("", "No account found with that email.");
                return View(model);
            }

            // Redirect to SecurityQuestion with email in query string
            return RedirectToAction("SecurityQuestion", new { email = customer.Email });
        }

        // ---------------------------
        // SecurityQuestion (show question) GET and VerifyAnswer POST
        // ---------------------------
        [HttpGet]
        public async Task<IActionResult> SecurityQuestion(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return RedirectToAction("ForgetPassword");

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null) return RedirectToAction("ForgetPassword");

            var vm = new SecurityQuestionViewModel
            {
                Email = customer.Email,
                SecurityQuestion = customer.SecurityQuestion
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyAnswer(SecurityQuestionViewModel model)
        {
            if (!ModelState.IsValid) return View("SecurityQuestion", model);

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
            if (customer == null) return RedirectToAction("ForgetPassword");

            using var sha256 = SHA256.Create();
            var providedHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(model.SecurityAnswer.ToLower().Trim())));

            if (providedHash != customer.SecurityAnswerHash)
            {
                ModelState.AddModelError("", "Incorrect answer.");
                model.SecurityQuestion = customer.SecurityQuestion;
                return View("SecurityQuestion", model);
            }

            // Answer ok -> go to reset password page (email in query string)
            return RedirectToAction("ResetPassword", new { email = customer.Email });
        }

        // ---------------------------
        // Reset Password (GET + POST)
        // ---------------------------
        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return RedirectToAction("ForgetPassword");

            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Account not found.");
                return View(model);
            }

            // generate token then immediately apply it (no email needed)
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (result.Succeeded)
            {
                TempData["Message"] = "Password updated. Please log in.";
                return RedirectToAction("Login");
            }

            foreach (var err in result.Errors) ModelState.AddModelError("", err.Description);
            return View(model);
        }
    }
}
