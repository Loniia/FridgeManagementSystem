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
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly FridgeDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ManageEmployeeController> _logger;

        public ManageEmployeeController(
            FridgeDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<ManageEmployeeController> logger,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
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
                // Show Identity errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                PopulateDropdowns();
                return View(model);
            }

            // 2️⃣ Ensure Employee role exists
            if (!await _roleManager.RoleExistsAsync(Roles.Employee))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(Roles.Employee));
            }

            // 3️⃣ Add user to Employee role
            await _userManager.AddToRoleAsync(user, Roles.Employee);

            // 4️⃣ Create Employee record
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

         [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        PopulateDropdowns();
        return View(employee);
    }

    // ✅ POST: Edit Employee
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Employee model)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(model);
        }

        var employee = await _context.Employees.FindAsync(model.EmployeeID);
        if (employee == null) return NotFound();

        // Update employee fields
        employee.FullName = model.FullName;
        employee.Email = model.Email;
        employee.PhoneNumber = model.PhoneNumber;
        employee.Address = model.Address;
        employee.City = model.City;
        employee.PostalCode = model.PostalCode;
        employee.Role = model.Role;
        employee.HireDate = model.HireDate;
        employee.PayRate = model.PayRate;
        employee.LocationId = model.LocationId;
        employee.Status = model.Status;

        _context.Update(employee);
        await _context.SaveChangesAsync();

        // Update linked Identity user (email & fullname)
        var user = await _userManager.FindByIdAsync(employee.ApplicationUserId.ToString());
        if (user != null)
        {
            user.Email = model.Email;
            user.UserName = model.Email;
            user.FullName = model.FullName;
            await _userManager.UpdateAsync(user);
        }

        TempData["SuccessMessage"] = "Employee updated successfully!";
        return RedirectToAction(nameof(Index));
    }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Location)
                .Include(e => e.UserAccount)
                .Include(e => e.Faults)
                .FirstOrDefaultAsync(e => e.EmployeeID == id && e.Status != "Deleted");

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        // ✅ Soft Delete Employee (set Status = "Inactive")
        [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        employee.Status = "Inactive";
        _context.Update(employee);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Employee deactivated (soft deleted) successfully!";
        return RedirectToAction(nameof(Index));
    }

    // ✅ Helper — populate dropdown data
    //private void PopulateDropdowns()
    //{
    //    // Roles
    //    var roles = new[]
    //    {
    //        new { Value = EmployeeRoles.CustomerManager, Text = "Customer Manager" },
    //        new { Value = EmployeeRoles.FaultTechnician, Text = "Fault Technician" },
    //        new { Value = EmployeeRoles.MaintenanceTechnician, Text = "Maintenance Technician" },
    //        new { Value = EmployeeRoles.PurchasingManager, Text = "Purchasing Manager" }
    //    };
    //    ViewBag.Roles = new SelectList(roles, "Value", "Text");

    //    // Locations
    //    var locations = _context.Locations
    //        .Where(l => l.IsActive)
    //        .Select(l => new
    //        {
    //            l.LocationId,
    //            FullAddress = l.Address + ", " + l.City
    //        })
    //        .ToList();

    //    ViewBag.Locations = new SelectList(locations, "LocationId", "FullAddress");
    //}
    }
}
