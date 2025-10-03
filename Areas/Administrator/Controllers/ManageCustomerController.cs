using Microsoft.AspNetCore.Mvc;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = Roles.Admin)]
    public class ManageCustomerController : Controller
    {
        private readonly CustomerService _customerService;

        public ManageCustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // --------------------------
        // Combined View: List + Select Customer
        // --------------------------
        public async Task<IActionResult> ManageCustomers(int? selectedCustomerId)
        {
            var customers = await _customerService.GetAllCustomersWithFridgesAsync();

            Customer selectedCustomer = null;
            if (selectedCustomerId.HasValue)
            {
                selectedCustomer = await _customerService.GetCustomerDetailsAsync(selectedCustomerId.Value);
            }

            var viewModel = new ManageCustomersViewModel
            {
                AllCustomers = customers,
                SelectedCustomer = selectedCustomer
            };

            return View(viewModel);
        }

        // --------------------------
        // Index List
        // --------------------------
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersWithFridgesAsync();
            return View(customers);
        }

        // --------------------------
        // Dashboard List (like Takealot admin view)
        // --------------------------
        public async Task<IActionResult> DashboardList()
        {
            var customers = await _customerService.GetAllCustomersWithFridgesAsync();
            return View("DashboardList", customers);
        }

        // --------------------------
        // Single Customer Dashboard
        // --------------------------
        public async Task<IActionResult> Dashboard(int id)
        {
            var customer = await _customerService.GetCustomerDetailsAsync(id);

            if (customer == null) return NotFound();

            return View(customer);
        }

        // --------------------------
        // Create Customer
        // --------------------------
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerService.AddCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // --------------------------
        // Edit Customer
        // --------------------------
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetCustomerDetailsAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.CustomerID) return NotFound();

            if (ModelState.IsValid)
            {
                await _customerService.UpdateCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // --------------------------
        // Delete Customer
        // --------------------------
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetCustomerDetailsAsync(id);
            if (customer == null) return NotFound();

            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}


//using Microsoft.AspNetCore.Mvc;
//using FridgeManagementSystem.Data;
//using FridgeManagementSystem.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.EntityFrameworkCore;

//#nullable disable

//namespace FridgeManagementSystem.Areas.Administrator.Controllers
//{
//    [Area("Administrator")]
//    [Authorize(Roles = Roles.Admin)]
//    public class ManageCustomerController : Controller
//    {
//        private readonly FridgeDbContext _context;

//        public ManageCustomerController(FridgeDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult ManageCustomers(int? selectedCustomerId)
//        {
//            var customers = _context.Customers.ToList();

//            Customer selectedCustomer = null;
//            if (selectedCustomerId.HasValue)
//            {
//                selectedCustomer = _context.Customers
//                    .Include(c => c.FridgeAllocation).ThenInclude(f => f.Fridge)
//                    .Include(c => c.FaultReports)
//                    .Include(c => c.CustomerNote)
//                    .FirstOrDefault(c => c.CustomerID == selectedCustomerId.Value);
//            }

//            var viewModel = new ManageCustomersViewModel
//            {
//                AllCustomers = customers,
//                SelectedCustomer = selectedCustomer
//            };

//            return View(viewModel);
//        }

//        public async Task<IActionResult> Index()
//        {
//            var customers = await _context.Customers.ToListAsync();
//            return View(customers);
//        }

//        // GET: /Administrator/ManageCustomer/Dashboard
//        public async Task<IActionResult> DashboardList()
//        {
//            var customers = await _context.Customers.ToListAsync();
//            return View("DashboardList", customers); 
//        }

//        public async Task<IActionResult> Dashboard(int id)
//        {
//            var customer = await _context.Customers
//                .Include(c => c.FridgeAllocation)
//                .ThenInclude(fa => fa.Fridge)
//                .Include(c => c.FaultReports)
//                .Include(c => c.CustomerNote)
//                .FirstOrDefaultAsync(c => c.CustomerID == id);

//            if (customer == null) return NotFound();

//            return View(customer);
//        }

//        public IActionResult Create() => View();

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Customer customer)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(customer);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(customer);
//        }

//        public async Task<IActionResult> Edit(int id)
//        {
//            var customer = await _context.Customers.FindAsync(id);
//            if (customer == null) return NotFound();
//            return View(customer);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, Customer customer)
//        {
//            if (id != customer.CustomerID) return NotFound();

//            if (ModelState.IsValid)
//            {
//                _context.Update(customer);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(customer);
//        }

//        public async Task<IActionResult> Delete(int id)
//        {
//            var customer = await _context.Customers.FindAsync(id);
//            if (customer == null) return NotFound();

//            _context.Customers.Remove(customer);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}