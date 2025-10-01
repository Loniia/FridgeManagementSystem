using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Controllers
{
    [Authorize(Roles = "Customer")]
        public class CustomerController : Controller
        {
            private readonly FridgeDbContext _context;

            public CustomerController(FridgeDbContext context)
            {
                _context = context;
            }

        //Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // GET: Customer
        public async Task<IActionResult> Index()
            {
                var customers = await _context.Customers
                    .Include(c => c.UserAccount)
                    .ToListAsync();
                return View(customers);
            }

            // GET: Customer/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null) return NotFound();

                var customer = await _context.Customers
                    .Include(c => c.UserAccount)
                    .FirstOrDefaultAsync(m => m.CustomerID == id);

                if (customer == null) return NotFound();

                return View(customer);
            }

            // GET: Customer/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Customer/Create
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

            // GET: Customer/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null) return NotFound();

                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return NotFound();

                return View(customer);
            }

            // POST: Customer/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, Customer customer)
            {
                if (id != customer.CustomerID) return NotFound();

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(customer);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_context.Customers.Any(e => e.CustomerID == id))
                            return NotFound();
                        else
                            throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(customer);
            }

            // GET: Customer/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null) return NotFound();

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(m => m.CustomerID == id);

                if (customer == null) return NotFound();

                return View(customer);
            }

            // POST: Customer/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }

        // Receive & Track Fridges
        public IActionResult ReceiveTrack()
        {
            var fridges = _context.Fridge
                .ToList(); 
            return View(fridges);
        }
        // customer views service history 
        public IActionResult MyServiceHistory(int customerId)
        {
            var visits = _context.MaintenanceVisit
                .Include(v => v.Fridge)
                .Include(v => v.MaintenanceChecklist)
                .Include(v => v.ComponentUsed)
                .Include(v => v.FaultReport)
                .Where(v => v.Fridge.CustomerId == customerId)
                .OrderByDescending(v => v.ScheduledDate)
                .ToList();

            return View(visits);
        }
        // customer views upcoming maintenance visits
        public IActionResult MyUpcomingVisits(int customerId)
        {
            var visits = _context.MaintenanceVisit
                .Include(v => v.Fridge)
                .Where(v => v.Fridge.CustomerId == customerId &&
                            (v.Status == Models.TaskStatus.Scheduled || v.Status == Models.TaskStatus.Rescheduled))
                .OrderBy(v => v.ScheduledDate)
                .ToList();

            return View(visits);
        }

        //// Create Faults
        //public IActionResult CreateFault()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult CreateFault(Fault model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Faults.Add(model);
        //        _context.SaveChanges();
        //        return RedirectToAction("Dashboard");
        //    }
        //    return View(model);
        //}

        //// Maintenance Visit
        //public IActionResult MaintenanceVisit()
        //{
        //    var visits = _context.MaintenanceVisits.ToList(); // example
        //    return View(visits);
        //}

        //// View Fridge History
        //public IActionResult FridgeHistory()
        //{
        //    var history = _context.FridgeHistories.ToList(); // example
        //    return View(history);
        //}

        //// Create Fridge Request
        //public IActionResult CreateFridgeRequest()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult CreateFridgeRequest(FridgeRequest model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.FridgeRequests.Add(model);
        //        _context.SaveChanges();
        //        return RedirectToAction("Dashboard");
        //    }
        //    return View(model);
        //}
    }
}
