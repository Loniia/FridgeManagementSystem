using FridgeManagementSystem.Data; // Your DbContext
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class ManageEmployeeController : Controller
    {
        private readonly ILogger<ManageEmployeeController> _logger; 
        private readonly FridgeDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageEmployeeController(FridgeDbContext context, UserManager<ApplicationUser> userManager, ILogger<ManageEmployeeController> logger )
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        public IActionResult Dashboard() 
        {
            return View();
        }

        // GET: ManageEmployees Page
        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            ViewBag.Locations = _context.Locations
                .Select(l => new SelectListItem
                {
                    Text = l.Address + ", " + l.City,
                    Value = l.LocationId.ToString()
                }).ToList();

            return View(employees);
        }
        // GET: Add New Employee
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View(new Employee());
        }

        // POST: Add New Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(model);
            }

            // Create ApplicationUser
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

            // Save Employee record
            model.ApplicationUserId = user.Id;
            _context.Employees.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Employee registered successfully!";
            return RedirectToAction("Index");
        }

        // ---------------- Helper to populate dropdowns ----------------
        private void PopulateDropdowns()
        {
            // Roles linked to subsystem
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Customer Manager", Value = EmployeeRoles.CustomerManager },
                new SelectListItem { Text = "Fault Technician", Value = EmployeeRoles.FaultTechnician },
                new SelectListItem { Text = "Maintenance Technician", Value = EmployeeRoles.MaintenanceTechnician },
                new SelectListItem { Text = "Purchasing Manager", Value = EmployeeRoles.PurchasingManager }
            };

            // Locations dropdown
            ViewBag.Locations = _context.Locations
                .Where(l => l.IsActive)
                .Select(l => new SelectListItem
                {
                    Text = l.Address + ", " + l.City,
                    Value = l.LocationId.ToString()
                })
                .ToList();
        }
    }
}

