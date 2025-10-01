using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace FridgeManagementSystem.Controllers
{
    [Area("FaultTechSubsystem")]
    public class FaultsController : Controller
    {
        private readonly FridgeDbContext _context;

        public FaultsController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: Faults - View Faults
        public async Task<IActionResult> Index()
        {
            var faults = await _context.Faults
                .Include(f => f.Fridge)
                .Include(f => f.Fridge.Customer)
                .OrderByDescending(f => f.FaultID)
                .ToListAsync();

            return View(faults);
        }

// GET: Faults/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);

            var dashboardData = new DashboardViewModel
            {
                TotalFaults = await _context.Faults.CountAsync(),
                HighPriorityFaults = await _context.Faults.CountAsync(f => f.Priority == "High"),
                UnattendedFaults = await _context.Faults.CountAsync(f => f.Status == "Pending"),
                TodaysRepairs = await _context.RepairSchedules
                    .CountAsync(r => r.RepairStartDate.HasValue && 
                                    r.RepairStartDate.Value.Date == today),
                RecentFaults = await _context.Faults
                    .Include(f => f.Fridge)
                    .Include(f => f.Fridge.Customer)
                    .Include(f => f.Technician)
                    .OrderByDescending(f => f.FaultID)
                    .Take(5)
                    .ToListAsync(),
                CompletedThisWeek = await _context.Faults
                    .CountAsync(f => f.Status == "Resolved" && 
                                    f.UpdatedDate >= startOfWeek)
            };

            return View(dashboardData);
        }

        // GET: Faults/Process/5 - Process Fault
        public async Task<IActionResult> Process(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults
                .Include(f => f.Fridge)
                .Include(f => f.Fridge.Customer)
                .FirstOrDefaultAsync(m => m.FaultID == id);

            if (fault == null)
            {
                return NotFound();
            }

            // Prepare repair schedule if it doesn't exist
            var repairSchedule = await _context.RepairSchedules
                .FirstOrDefaultAsync(r => r.FaultID == id) ?? new RepairSchedule
                {
                    FaultID = fault.FaultID,
                    FridgeId = fault.FridgeId,
                    Status = "Diagnosing"
                };

            ViewBag.FaultInfo = fault;
            ViewBag.StatusOptions = new SelectList(new[]
            {
                new { Value = "Diagnosing", Text = "Diagnosing" },
                new { Value = "Awaiting Parts", Text = "Awaiting Parts" },
                new { Value = "Repairing", Text = "Repairing" },
                new { Value = "Testing", Text = "Testing" },
                new { Value = "Completed", Text = "Completed" }
            }, "Value", "Text", repairSchedule.Status);

            ViewBag.ConditionOptions = new SelectList(new[]
            {
                new { Value = "Working", Text = "Working" },
                new { Value = "Under Repair", Text = "Under Repair" },
                new { Value = "Faulty", Text = "Faulty" },
                new { Value = "Scrapped", Text = "Scrapped" }
            }, "Value", "Text", fault.Fridge?.Condition ?? "Working");

            return View(repairSchedule);
        }

        // POST: Faults/Process/5 - Process Fault
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(int id, [Bind("RepairID,Diagnosis,RepairNotes,Status,RepairDate,FaultID,FridgeID")] RepairSchedule repairSchedule, string fridgeCondition)
        {
            if (id != repairSchedule.FaultID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update or create repair schedule
                    var existingRepair = await _context.RepairSchedules
                        .FirstOrDefaultAsync(r => r.FaultID == id);

                    if (existingRepair != null)
                    {
                        existingRepair.Diagnosis = repairSchedule.Diagnosis;
                        existingRepair.RepairNotes = repairSchedule.RepairNotes;
                        existingRepair.Status = repairSchedule.Status;
                        existingRepair.UpdatedDate = DateTime.Now;
                        _context.Update(existingRepair);
                    }
                    else
                    {
                        repairSchedule.CreatedDate = DateTime.Now;
                        repairSchedule.UpdatedDate = DateTime.Now;
                        _context.Add(repairSchedule);
                    }

                    // Update fault status based on repair status
                    var fault = await _context.Faults.FindAsync(id);
                    if (fault != null)
                    {
                        fault.Status = repairSchedule.Status == "Completed" ? "Resolved" : "In Progress";
                        fault.UpdatedDate = DateTime.Now;
                        _context.Update(fault);
                    }

                    // Update fridge condition
                    var fridge = await _context.Fridge.FindAsync(repairSchedule.FridgeId);
                    if (fridge != null && !string.IsNullOrEmpty(fridgeCondition))
                    {
                        fridge.Condition = fridgeCondition;
                        fridge.UpdatedDate = DateTime.Now;
                        _context.Update(fridge);
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Fault processing updated successfully!";

                    if (repairSchedule.Status == "Completed")
                    {
                        return RedirectToAction("Complete", new { id = repairSchedule.RepairID });
                    }

                    return RedirectToAction(nameof(Index));
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
            }

            // Repopulate view data if validation fails
            var faultInfo = await _context.Faults
                .Include(f => f.Fridge)
                .FirstOrDefaultAsync(f => f.FaultID == id);

            ViewBag.FaultInfo = faultInfo;
            ViewBag.StatusOptions = new SelectList(new[]
            {
                new { Value = "Diagnosing", Text = "Diagnosing" },
                new { Value = "Awaiting Parts", Text = "Awaiting Parts" },
                new { Value = "Repairing", Text = "Repairing" },
                new { Value = "Testing", Text = "Testing" },
                new { Value = "Completed", Text = "Completed" }
            }, "Value", "Text", repairSchedule.Status);

            ViewBag.ConditionOptions = new SelectList(new[]
            {
                new { Value = "Working", Text = "Working" },
                new { Value = "Under Repair", Text = "Under Repair" },
                new { Value = "Faulty", Text = "Faulty" },
                new { Value = "Scrapped", Text = "Scrapped" }
            }, "Value", "Text", fridgeCondition);

            return View(repairSchedule);
        }

        // GET: Faults/Repair/5 - Repair Fridge
        public async Task<IActionResult> Repair(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults
                .Include(f => f.Fridge)
                .Include(f => f.Fridge.Customer)
                .FirstOrDefaultAsync(m => m.FaultID == id);

            if (fault == null)
            {
                return NotFound();
            }

            var repairSchedule = await _context.RepairSchedules
                .FirstOrDefaultAsync(r => r.FaultID == id) ?? new RepairSchedule
                {
                    FaultID = fault.FaultID,
                    FridgeId = fault.FridgeId,
                    Status = "Repairing"
                };

            ViewBag.FaultInfo = fault;
            ViewBag.RepairTypes = new SelectList(new[]
            {
                new { Value = "Electrical", Text = "Electrical Repair" },
                new { Value = "Mechanical", Text = "Mechanical Repair" },
                new { Value = "Refrigeration", Text = "Refrigeration System" },
                new { Value = "Thermostat", Text = "Thermostat Replacement" },
                new { Value = "Compressor", Text = "Compressor Repair" },
                new { Value = "Other", Text = "Other" }
            }, "Value", "Text");

            return View(repairSchedule);
        }

        // POST: Faults/Repair/5 - Repair Fridge
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Repair(int id, [Bind("RepairID,RepairType,PartsUsed,RepairNotes,RepairCost,Status,FaultID,FridgeID")] RepairSchedule repairSchedule)
        {
            if (id != repairSchedule.FaultID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRepair = await _context.RepairSchedules
                        .FirstOrDefaultAsync(r => r.FaultID == id);

                    if (existingRepair != null)
                    {
                        existingRepair.RepairType = repairSchedule.RepairType;
                        existingRepair.PartsUsed = repairSchedule.PartsUsed;
                        existingRepair.RepairNotes = repairSchedule.RepairNotes;
                        existingRepair.RepairCost = repairSchedule.RepairCost;
                        existingRepair.Status = "Testing"; // Move to testing after repair
                        existingRepair.UpdatedDate = DateTime.Now;
                        _context.Update(existingRepair);
                    }
                    else
                    {
                        repairSchedule.Status = "Testing";
                        repairSchedule.CreatedDate = DateTime.Now;
                        repairSchedule.UpdatedDate = DateTime.Now;
                        _context.Add(repairSchedule);
                    }

                    // Update fault status
                    var fault = await _context.Faults.FindAsync(id);
                    if (fault != null)
                    {
                        fault.Status = "In Progress";
                        fault.UpdatedDate = DateTime.Now;
                        _context.Update(fault);
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Repair details saved successfully! Moving to testing phase.";
                    return RedirectToAction("Process", new { id = id });
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
            }

            var faultInfo = await _context.Faults
                .Include(f => f.Fridge)
                .FirstOrDefaultAsync(f => f.FaultID == id);

            ViewBag.FaultInfo = faultInfo;
            ViewBag.RepairTypes = new SelectList(new[]
            {
                new { Value = "Electrical", Text = "Electrical Repair" },
                new { Value = "Mechanical", Text = "Mechanical Repair" },
                new { Value = "Refrigeration", Text = "Refrigeration System" },
                new { Value = "Thermostat", Text = "Thermostat Replacement" },
                new { Value = "Compressor", Text = "Compressor Repair" },
                new { Value = "Other", Text = "Other" }
            }, "Value", "Text", repairSchedule.RepairType);

            return View(repairSchedule);
        }

        // GET: Faults/UpdateCondition/5 - Update Fridge Condition
        public async Task<IActionResult> UpdateCondition(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults
                .Include(f => f.Fridge)
                .Include(f => f.Fridge.Customer)
                .FirstOrDefaultAsync(m => m.FaultID == id);

            if (fault == null || fault.Fridge == null)
            {
                return NotFound();
            }

            ViewBag.FaultInfo = fault;
            ViewBag.ConditionOptions = new SelectList(new[]
            {
                new { Value = "Working", Text = "Working - Fully Functional" },
                new { Value = "Under Repair", Text = "Under Repair" },
                new { Value = "Faulty", Text = "Faulty - Needs Attention" },
                new { Value = "Scrapped", Text = "Scrapped - Beyond Repair" }
            }, "Value", "Text", fault.Fridge.Condition);

            return View(fault.Fridge);
        }

        // POST: Faults/UpdateCondition/5 - Update Fridge Condition
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCondition(int id, [Bind("FridgeID,Model,SerialNumber,Brand,Type,Condition,Notes")] Fridge fridge)
        {
            if (id != fridge.FridgeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingFridge = await _context.Fridge.FindAsync(id);
                    if (existingFridge != null)
                    {
                        existingFridge.Condition = fridge.Condition;
                        existingFridge.Notes = fridge.Notes;
                        existingFridge.UpdatedDate = DateTime.Now;
                        _context.Update(existingFridge);

                        // If condition is set to "Working", check if we should mark fault as resolved
                        if (fridge.Condition == "Working")
                        {
                            var relatedFault = await _context.Faults
                                .FirstOrDefaultAsync(f => f.FridgeId == id && f.Status != "Resolved");

                            if (relatedFault != null)
                            {
                                relatedFault.Status = "Resolved";
                                relatedFault.UpdatedDate = DateTime.Now;
                                _context.Update(relatedFault);
                            }
                        }

                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = $"Fridge condition updated to '{fridge.Condition}' successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FridgeExists(fridge.FridgeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var faultInfo = await _context.Faults
                .Include(f => f.Fridge)
                .FirstOrDefaultAsync(f => f.FridgeId == id);

            ViewBag.FaultInfo = faultInfo;
            ViewBag.ConditionOptions = new SelectList(new[]
            {
                new { Value = "Working", Text = "Working - Fully Functional" },
                new { Value = "Under Repair", Text = "Under Repair" },
                new { Value = "Faulty", Text = "Faulty - Needs Attention" },
                new { Value = "Scrapped", Text = "Scrapped - Beyond Repair" }
            }, "Value", "Text", fridge.Condition);

            return View(fridge);
        }

        // GET: Faults/Complete/5 - Complete Repair Process
        public async Task<IActionResult> Complete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairSchedule = await _context.RepairSchedules
                .Include(r => r.Fault)
                .Include(r => r.Fridge)
                .Include(r => r.Fridge.Customer)
                .FirstOrDefaultAsync(m => m.RepairID == id);

            if (repairSchedule == null)
            {
                return NotFound();
            }

            return View(repairSchedule);
        }

        // POST: Faults/Complete/5 - Complete Repair Process
        [HttpPost, ActionName("Complete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteConfirmed(int id)
        {
            var repairSchedule = await _context.RepairSchedules
                .Include(r => r.Fault)
                .Include(r => r.Fridge)
                .FirstOrDefaultAsync(r => r.RepairID == id);

            if (repairSchedule != null)
            {
                repairSchedule.Status = "Completed";
                repairSchedule.UpdatedDate = DateTime.Now;

                // Update fault status
                if (repairSchedule.Fault != null)
                {
                    repairSchedule.Fault.Status = "Resolved";
                    repairSchedule.Fault.UpdatedDate = DateTime.Now;
                    _context.Update(repairSchedule.Fault);
                }

                // Update fridge condition to working
                if (repairSchedule.Fridge != null)
                {
                    repairSchedule.Fridge.Condition = "Working";
                    repairSchedule.Fridge.UpdatedDate = DateTime.Now;
                    _context.Update(repairSchedule.Fridge);
                }

                _context.Update(repairSchedule);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Repair completed successfully! Fridge is now working.";
            }

            return RedirectToAction(nameof(Index));
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
                .Include(f => f.Fridge.Customer)
                .Include(f => f.RepairSchedules)
                .FirstOrDefaultAsync(m => m.FaultID == id);

            if (fault == null)
            {
                return NotFound();
            }

            return View(fault);
        }

        private bool FaultExists(int id)
        {
            return _context.Faults.Any(e => e.FaultID == id);
        }

        private bool RepairScheduleExists(int id)
        {
            return _context.RepairSchedules.Any(e => e.RepairID == id);
        }

        private bool FridgeExists(int id)
        {
            return _context.Fridge.Any(e => e.FridgeId == id);
        }
    }

    // Dashboard ViewModel class
    public class DashboardViewModel
    {
        public int TotalFaults { get; set; }
        public int HighPriorityFaults { get; set; }
        public int UnattendedFaults { get; set; }
        public int TodaysRepairs { get; set; }
        public int CompletedThisWeek { get; set; }
        public List<Fault> RecentFaults { get; set; } = new List<Fault>();
    }
}