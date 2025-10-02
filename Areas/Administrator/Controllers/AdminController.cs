using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly FridgeDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, FridgeDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View("Dashboard");
        }

        public IActionResult Dashboard()
        {
            return View();
        }


        // List all employees
        public async Task<IActionResult> ManageEmployees()
        {
            var employees = await _context.Employees
                .Include(e => e.UserAccount)
                .ToListAsync();

            return View(employees);
        }

        // ✅ GET: Create Employee
        public IActionResult CreateEmployee()
        {
            return View(new RegisterEmployeeViewModel());
        }

        // ✅ POST: Create Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(RegisterEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Create ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    UserType = Roles.Employee,
                    EmployeeRole = model.EmployeeRole // <-- important
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Ensure "Employee" role exists
                    if (!await _roleManager.RoleExistsAsync(Roles.Employee))
                    {
                        await _roleManager.CreateAsync(new IdentityRole<int>(Roles.Employee));
                    }

                    // Assign role
                    await _userManager.AddToRoleAsync(user, Roles.Employee);

                    // Create Employee entity
                    var employee = new Employee
                    {
                        FullName = model.FullName,
                        ContactInfo = model.Email,
                        Role = model.EmployeeRole, // <-- store subsystem role
                        Status = "Active",
                        ApplicationUserId = user.Id
                    };

                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ManageEmployees));
                }

                // If errors from Identity
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }

            return View(model);
        }
    }
}
