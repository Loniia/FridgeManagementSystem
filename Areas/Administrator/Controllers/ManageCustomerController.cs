using Microsoft.AspNetCore.Mvc;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;    
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    public class ManageCustomerController : Controller
    {
        public class CustomerManagementController : Controller
        {
            private readonly FridgeDbContext _context;

            public CustomerManagementController(FridgeDbContext context)
            {
                _context = context;
            }

            public IActionResult ManageCustomers(int? selectedCustomerId)
            {
                var customers = _context.Customers.ToList();

                Customer selectedCustomer = null;
                if (selectedCustomerId.HasValue)
                {
                    selectedCustomer = _context.Customers
                        .Include(c => c.FridgeAllocation).ThenInclude(f => f.fridge)
                        .Include(c => c.FaultReports)
                        .Include(c => c.ServiceHistories)
                        .Include(c => c.CustomerNote)
                        .FirstOrDefault(c => c.CustomerID == selectedCustomerId.Value);
                }

                var viewModel = new ManageCustomersViewModel
                {
                    AllCustomers = customers,
                    SelectedCustomer = selectedCustomer
                };

                return View(viewModel);
            }


            //Show all customers (like Admin’s customer list)
            public async Task<IActionResult> Index()
            {
                var customers = await _context.Customers.ToListAsync();
                return View(customers);
            }

            // Dashboard for ONE customer (like Takealot My Account)
            public async Task<IActionResult> Dashboard(int id)
            {
                var customer = await _context.Customers
                    .Include(c => c.FridgeAllocation)
                    .ThenInclude(fa => fa.fridge)
                    .Include(c => c.FaultReports)
                    .Include(c => c.ServiceHistories)
                    .Include(c => c.CustomerNote)
                    .FirstOrDefaultAsync(c => c.CustomerID == id);

                if (customer == null) return NotFound();

                return View(customer);
            }

            //Add or Edit customer profile
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(Customer customer)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(customer);
            }

            public async Task<IActionResult> Edit(int id)
            {
                var customer = await _context.Customers.FindAsync(id);
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
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(customer);
            }

            // 🔹 Delete customer
            public async Task<IActionResult> Delete(int id)
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return NotFound();

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
