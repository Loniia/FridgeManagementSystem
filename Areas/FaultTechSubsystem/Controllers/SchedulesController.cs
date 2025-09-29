using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace FridgeManagementSystem.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly FridgeDbContext _context;

        public SchedulesController(FridgeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var schedules = await _context.RepairSchedules
                .Include(r => r.Fault)   // if linked
                .ToListAsync();

            return View(schedules);
        }
        // GET: Schedules/Schedule/5 - Specific action for scheduling from a fault
        public async Task<IActionResult> Schedule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults
                .Include(f => f.Fridge)
                .FirstOrDefaultAsync(f => f.FaultID == id);

            if (fault == null)
            {
                return NotFound();
            }

            // Create a new repair schedule pre-populated with fault information
            var repairSchedule = new RepairSchedule
            {
                FaultID = fault.FaultID,
                FridgeID = fault.FridgeId,
                Status = "Scheduled",
                Diagnosis = fault.FaultDescription
            };

            ViewData["FaultID"] = new SelectList(_context.Faults, "FaultID", "FaultDescription", fault.FaultID);
            ViewData["TechnicianID"] = new SelectList(_context.FaultTechnicians, "TechnicianID", "Name");
            ViewData["FridgeID"] = new SelectList(_context.Fridges, "FridgeID", "Model", fault.FridgeId);

            // Pass fault information to view for display
            ViewBag.FaultInfo = fault;

            return View(repairSchedule);
        }

        // POST: Schedules/Schedule/5 - Specific action for scheduling from a fault
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Schedule(int id, [Bind("RepairID,Diagnosis,RepairDate,RepairTime,Status,FaultID,TechnicianID,FridgeID")] RepairSchedule repairSchedule)
        {

            if (id != repairSchedule.FaultID)
            {
                return NotFound();
            }

            // Convert DateOnly to DateTime for comparison
            var repairDateAsDateTime = repairSchedule.RepairDate.ToDateTime(TimeOnly.MinValue);
            var today = DateTime.Today;


            // Validate repair date is not in the past and not on weekend
            if (repairDateAsDateTime < today)
            {
                ModelState.AddModelError("RepairDate", "Repair date cannot be in the past.");
            }
            else if (repairDateAsDateTime.DayOfWeek == DayOfWeek.Saturday || repairDateAsDateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("RepairDate", "Repairs cannot be scheduled on weekends.");
            }


            // Validate repair time is within working hours (8 AM to 5 PM)
            if (repairSchedule.ScheduleTime != null)
            {
                var repairTime = repairSchedule.ScheduleTime;
                if (repairTime.Hours < 8 || repairTime.Hours >= 17)
                {
                    ModelState.AddModelError("RepairTime", "Repairs can only be scheduled between 8:00 AM and 5:00 PM.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the fault status to "In Progress" when scheduling repair
                    var fault = await _context.Faults.FindAsync(id);
                    if (fault != null)
                    {
                        fault.Status = "In Progress";
                        _context.Update(fault);
                    }

                    _context.Add(repairSchedule);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Repair scheduled successfully!";
                    return RedirectToAction("Index", "Faults");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            // If we got this far, something failed; redisplay form
            var faultInfo = await _context.Faults
                .Include(f => f.Fridge)
                .FirstOrDefaultAsync(f => f.FaultID == id);

            ViewBag.FaultInfo = faultInfo;
            ViewData["FaultID"] = new SelectList(_context.Faults, "FaultID", "FaultDescription", repairSchedule.FaultID);
            ViewData["TechnicianID"] = new SelectList(_context.FaultTechnicians, "TechnicianID", "Name", repairSchedule.TechnicianID);
            ViewData["FridgeID"] = new SelectList(_context.Fridges, "FridgeID", "Model", repairSchedule.FridgeID);

            return View(repairSchedule);
        }
        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairSchedule = await _context.RepairSchedules
                .Include(r => r.Fault)
                .Include(r => r.FaultTechnician)
                .Include(r => r.Fridge)
                .FirstOrDefaultAsync(m => m.RepairID == id);

            if (repairSchedule == null)
            {
                return NotFound();
            }

            return View(repairSchedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            ViewData["FaultID"] = new SelectList(_context.Faults, "FaultID", "FaultDescription");
            ViewData["TechnicianID"] = new SelectList(_context.FaultTechnicians, "TechnicianID", "Name");
            ViewData["FridgeID"] = new SelectList(_context.Fridges, "FridgeID", "Model");
            return View();
        }


        // POST: Schedules/Create (Original create action)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RepairID,Diagnosis,RepairDate,RepairTime,Status,FaultID,TechnicianID,FridgeID")] RepairSchedule repairSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repairSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["FaultID"] = new SelectList(_context.Faults, "FaultID", "FaultDescription", repairSchedule.FaultID);
            ViewData["TechnicianID"] = new SelectList(_context.FaultTechnicians, "TechnicianID", "Name", repairSchedule.TechnicianID);
            ViewData["FridgeID"] = new SelectList(_context.Fridges, "FridgeID", "Model", repairSchedule.FridgeID);
            return View(repairSchedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairSchedule = await _context.RepairSchedules.FindAsync(id);
            if (repairSchedule == null)
            {
                return NotFound();
            }

            ViewData["FaultID"] = new SelectList(_context.Faults, "FaultID", "FaultDescription", repairSchedule.FaultID);
            ViewData["TechnicianID"] = new SelectList(_context.FaultTechnicians, "TechnicianID", "Name", repairSchedule.TechnicianID);
            ViewData["FridgeID"] = new SelectList(_context.Fridges, "FridgeID", "Model", repairSchedule.FridgeID);
            return View(repairSchedule);
        }

        // POST: Schedules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RepairID,Diagnosis,RepairDate,RepairTime,Status,FaultID,TechnicianID,FridgeID")] RepairSchedule repairSchedule)
        {
            if (id != repairSchedule.RepairID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repairSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairScheduleExists(repairSchedule.RepairID))
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

            ViewData["FaultID"] = new SelectList(_context.Faults, "FaultID", "FaultDescription", repairSchedule.FaultID);
            ViewData["TechnicianID"] = new SelectList(_context.FaultTechnicians, "TechnicianID", "Name", repairSchedule.TechnicianID);
            ViewData["FridgeID"] = new SelectList(_context.Fridges, "FridgeID", "Model", repairSchedule.FridgeID);
            return View(repairSchedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairSchedule = await _context.RepairSchedules
                .Include(r => r.Fault)
                .Include(r => r.FaultTechnician)
                .Include(r => r.Fridge)
                .FirstOrDefaultAsync(m => m.RepairID == id);

            if (repairSchedule == null)
            {
                return NotFound();
            }

            return View(repairSchedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repairSchedule = await _context.RepairSchedules.FindAsync(id);
            if (repairSchedule != null)
            {
                _context.RepairSchedules.Remove(repairSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Schedules/Complete/5 - Mark repair as completed
        public async Task<IActionResult> Complete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairSchedule = await _context.RepairSchedules
                .Include(r => r.Fault)
                .FirstOrDefaultAsync(m => m.RepairID == id);

            if (repairSchedule == null)
            {
                return NotFound();
            }

            return View(repairSchedule);
        }

        // POST: Schedules/Complete/5 - Mark repair as completed
        [HttpPost, ActionName("Complete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteConfirmed(int id)
        {
            var repairSchedule = await _context.RepairSchedules
                .Include(r => r.Fault)
                .FirstOrDefaultAsync(r => r.RepairID == id);

            if (repairSchedule != null)
            {
                repairSchedule.Status = "Completed";

                // Also update the fault status
                if (repairSchedule.Fault != null)
                {
                    repairSchedule.Fault.Status = "Resolved";
                    _context.Update(repairSchedule.Fault);
                }

                _context.Update(repairSchedule);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Repair marked as completed successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RepairScheduleExists(int id)
        {
            return _context.RepairSchedules.Any(e => e.RepairID == id);
        }
    }
}
