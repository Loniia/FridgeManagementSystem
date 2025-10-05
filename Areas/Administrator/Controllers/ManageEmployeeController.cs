using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class ManageEmployeeController : Controller
    {
        private readonly FridgeDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ManageEmployeeController> _logger;

        public ManageEmployeeController(
            FridgeDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<ManageEmployeeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        // ✅ Dashboard Page
        public IActionResult Dashboard()
        {
            return View();
        }

        // ✅ Index Page (Manage Employees)
        public IActionResult Index()
        {
            var employees = _context.Employees
                .Include(e => e.Location)
                .ToList();
            return View(employees);
        }

        // ✅ GET: Create Employee Form
        [HttpGet]
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View(new Employee());
        }

        // ✅ POST: Create Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(model);
            }

            // 1️⃣ Create linked Identity user
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                UserType = Roles.Employee,
                EmployeeRole = model.Role
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                PopulateDropdowns();
                return View(model);
            }

            // 2️⃣ Create Employee record
            model.ApplicationUserId = user.Id;
            _context.Employees.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Employee registered successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ✅ Helper — populate dropdown data
        private void PopulateDropdowns()
        {
            // Roles
            var roles = new[]
            {
                new { Value = EmployeeRoles.CustomerManager, Text = "Customer Manager" },
                new { Value = EmployeeRoles.FaultTechnician, Text = "Fault Technician" },
                new { Value = EmployeeRoles.MaintenanceTechnician, Text = "Maintenance Technician" },
                new { Value = EmployeeRoles.PurchasingManager, Text = "Purchasing Manager" }
            };
            ViewBag.Roles = new SelectList(roles, "Value", "Text");

            // Locations
            var locations = _context.Locations
                .Where(l => l.IsActive)
                .Select(l => new
                {
                    l.LocationId,
                    FullAddress = l.Address + ", " + l.City
                })
                .ToList();

            ViewBag.Locations = new SelectList(locations, "LocationId", "FullAddress");
        }
    }
}
