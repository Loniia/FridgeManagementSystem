using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace FridgeManagementSystem.Controllers
{
    public class FaultsController : Controller
    {
        private readonly FridgeDbContext _context;

        public FaultsController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: Faults - For Index view
        public async Task<IActionResult> Index()
        {
            var faults = await _context.Faults
                .Include(f => f.Fridge)
                .ToListAsync();

            return View(faults);
        }

        // GET: Faults/Dashboard - For Dashboard view
        public async Task<IActionResult> Dashboard()
        {
            var dashboardData = new DashboardViewModel
            {
                TotalFaults = await _context.Faults.CountAsync(),
                HighPriorityFaults = await _context.Faults.CountAsync(f => f.Priority == "High"),
                UnattendedFaults = await _context.Faults.CountAsync(f => f.Status == "Pending"),
                TodaysRepairs = await _context.RepairSchedules
                    .CountAsync(r => r.RepairDate == DateOnly.FromDateTime(DateTime.Today)),
                RecentFaults = await _context.Faults
                    .Include(f => f.Fridge)
                    .OrderByDescending(f => f.FaultID)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboardData);
        }

        // GET: Faults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults
                .Include(f => f.Fridge)
                .FirstOrDefaultAsync(m => m.FaultID == id);

            if (fault == null)
            {
                return NotFound();
            }

            return View(fault);
        }

        // GET: Faults/Create
        public IActionResult Create()
        {
            ViewData["FridgeID"] = new SelectList(_context.Fridges, "FridgeID", "Model");
            ViewBag.StatusOptions = new SelectList(new[]
            {
                new { Value = "Pending", Text = "Pending" },
                new { Value = "In Progress", Text = "In Progress" },
                new { Value = "Resolved", Text = "Resolved" },
                new { Value = "Closed", Text = "Closed" }
            }, "Value", "Text", "Pending");

            ViewBag.PriorityOptions = new SelectList(new[]
            {
                new { Value = "Low", Text = "Low" },
                new { Value = "Medium", Text = "Medium" },
                new { Value = "High", Text = "High" }
            }, "Value", "Text", "Low");

            return View();
        }

        // POST: Faults/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FaultID,FaultDescription,Status,Priority,FridgeID")] Fault fault)
        {
            if (ModelState.IsValid)
            {
                // Set default values if not provided
                if (string.IsNullOrEmpty(fault.Status))
                    fault.Status = "Pending";
                if (string.IsNullOrEmpty(fault.Priority))
                    fault.Priority = "Low";

                _context.Add(fault);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fault reported successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if validation fails
            ViewData["FridgeID"] = new SelectList(_context.Fridges, "FridgeID", "Model", fault.FridgeId);
            ViewBag.StatusOptions = new SelectList(new[]
            {
                new { Value = "Pending", Text = "Pending" },
                new { Value = "In Progress", Text = "In Progress" },
                new { Value = "Resolved", Text = "Resolved" },
                new { Value = "Closed", Text = "Closed" }
            }, "Value", "Text", fault.Status);

            ViewBag.PriorityOptions = new SelectList(new[]
            {
                new { Value = "Low", Text = "Low" },
                new { Value = "Medium", Text = "Medium" },
                new { Value = "High", Text = "High" }
            }, "Value", "Text", fault.Priority);

            return View(fault);
        }

        // GET: Faults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults.FindAsync(id);
            if (fault == null)
            {
                return NotFound();
            }

            ViewData["FridgeID"] = new SelectList(_context.Fridges, "FridgeID", "Model", fault.FridgeId);
            ViewBag.StatusOptions = new SelectList(new[]
            {
                new { Value = "Pending", Text = "Pending" },
                new { Value = "In Progress", Text = "In Progress" },
                new { Value = "Resolved", Text = "Resolved" },
                new { Value = "Closed", Text = "Closed" }
            }, "Value", "Text", fault.Status);

            ViewBag.PriorityOptions = new SelectList(new[]
            {
                new { Value = "Low", Text = "Low" },
                new { Value = "Medium", Text = "Medium" },
                new { Value = "High", Text = "High" }
            }, "Value", "Text", fault.Priority);

            return View(fault);
        }

        // POST: Faults/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FaultID,FaultDescription,Status,Priority,FridgeID")] Fault fault)
        {
            if (id != fault.FaultID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fault);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Fault updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaultExists(fault.FaultID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["FridgeID"] = new SelectList(_context.Fridges, "FridgeID", "Model", fault.FridgeId);
            ViewBag.StatusOptions = new SelectList(new[]
            {
                new { Value = "Pending", Text = "Pending" },
                new { Value = "In Progress", Text = "In Progress" },
                new { Value = "Resolved", Text = "Resolved" },
                new { Value = "Closed", Text = "Closed" }
            }, "Value", "Text", fault.Status);

            ViewBag.PriorityOptions = new SelectList(new[]
            {
                new { Value = "Low", Text = "Low" },
                new { Value = "Medium", Text = "Medium" },
                new { Value = "High", Text = "High" }
            }, "Value", "Text", fault.Priority);

            return View(fault);
        }

        // GET: Faults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults
                .Include(f => f.Fridge)
                .FirstOrDefaultAsync(m => m.FaultID == id);

            if (fault == null)
            {
                return NotFound();
            }

            return View(fault);
        }

        // POST: Faults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fault = await _context.Faults.FindAsync(id);
            if (fault != null)
            {
                _context.Faults.Remove(fault);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fault deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Faults/Schedule/5 - Redirect to Schedules controller
        public IActionResult Schedule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return RedirectToAction("Schedule", "Schedules", new { id = id });
        }

        // POST: Faults/UpdateStatus/5 - Update status only
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var fault = await _context.Faults.FindAsync(id);
            if (fault == null)
            {
                return NotFound();
            }

            fault.Status = status;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Status updated to {status} successfully!";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, newStatus = status });
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Faults/MarkComplete/5 - Mark fault as completed
        public async Task<IActionResult> MarkComplete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults.FindAsync(id);
            if (fault == null)
            {
                return NotFound();
            }

            return View(fault);
        }

        // POST: Faults/MarkComplete/5
        [HttpPost, ActionName("MarkComplete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCompleteConfirmed(int id)
        {
            var fault = await _context.Faults.FindAsync(id);
            if (fault != null)
            {
                fault.Status = "Resolved";
                _context.Update(fault);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fault marked as completed successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FaultExists(int id)
        {
            return _context.Faults.Any(e => e.FaultID == id);
        }
    }
    // Dashboard ViewModel class
    public class DashboardViewModel
    {
        public int TotalFaults { get; set; }
        public int HighPriorityFaults { get; set; }
        public int UnattendedFaults { get; set; }
        public int TodaysRepairs { get; set; }
        public List<Fault> RecentFaults { get; set; } = new List<Fault>();
    }
}