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
    [Route("Administrator/[controller]")]
    [Route("Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly FridgeDbContext _context;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            FridgeDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // Default page → Dashboard
        [HttpGet("Administrator/Admin")]
        [HttpGet("Administrator")]
        public IActionResult Index()
        {
            return View("Dashboard");
        }
        [HttpGet("Administrator/Admin/Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        // 🔹 List all employees
        [HttpGet("Administrator/Admin/ManageEmployees")]
        public async Task<IActionResult> ManageEmployees()
        {
            var employees = await _context.Employees
                .Include(e => e.UserAccount)
                .ToListAsync();

            return View(employees);
        }

        // 🔹 Details
        [HttpGet("Administrator/Admin/Details/{id}")]
        public async Task<IActionResult> DetailsEmployee(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.UserAccount)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (employee == null) return NotFound();

            return View(employee);
        }

        // 🔹 GET: Create
        [HttpGet("Administrator/Admin/CreateEmployee")]
        public IActionResult CreateEmployee()
        {
            return View(new RegisterEmployeeViewModel());
        }

        // 🔹 POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost("Administrator/Admin/CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(RegisterEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    UserType = Roles.Employee,
                    EmployeeRole = model.EmployeeRole
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(Roles.Employee))
                    {
                        await _roleManager.CreateAsync(new IdentityRole<int>(Roles.Employee));
                    }

                    await _userManager.AddToRoleAsync(user, Roles.Employee);

                    var employee = new Employee
                    {
                        FullName = model.FullName,
                        ContactInfo = model.Email,
                        Role = model.EmployeeRole,
                        Status = "Active",
                        ApplicationUserId = user.Id
                    };

                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ManageEmployees));
                }

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

        // 🔹 GET: Edit
        [HttpGet("Administrator/Admin/Edit/{id}")]
        public async Task<IActionResult> EditEmployee(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // 🔹 POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ManageEmployees));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Employees.Any(e => e.EmployeeID == id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(employee);
        }

        // 🔹 GET: Delete
        public async Task<IActionResult> DeleteEmployee(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.UserAccount)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (employee == null) return NotFound();

            return View(employee);
        }

        // 🔹 POST: Delete
        [HttpPost, ActionName("DeleteEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployeeConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageEmployees));
        }
    }
}

