using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FridgeManagementSystem.Controllers
{
    [Area("FaultTechSubsystem")]
    public class FaultsController : Controller
    {
        private readonly FridgeDbContext _context;
        private readonly ILogger<FaultsController> _logger;

        public FaultsController(FridgeDbContext context, ILogger<FaultsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Faults - View Fault Reports
        public async Task<IActionResult> Index()
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            var faultReports = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .Include(fr => fr.Fridge.Customer)
                .Include(fr => fr.MaintenanceVisit)
                .Include(fr => fr.RepairSchedules) // Direct repair schedules
                .Where(fr => fr.StatusFilter != "Resolved" && fr.StatusFilter != "Cancelled")
                .OrderByDescending(fr => fr.UrgencyLevel)
                .ThenByDescending(fr => fr.ReportDate)
                .ToListAsync();

            return View(faultReports);
        }

        // GET: Faults/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);

            var dashboardData = new DashboardViewModel
            {
                TotalFaults = await _context.FaultReport
                    .CountAsync(fr => fr.StatusFilter != "Resolved" && fr.StatusFilter != "Cancelled"),
                HighPriorityFaults = await _context.FaultReport
                    .CountAsync(fr => fr.UrgencyLevel == UrgencyLevel.High ||
                                     fr.UrgencyLevel == UrgencyLevel.Critical ||
                                     fr.UrgencyLevel == UrgencyLevel.Emergency),
                UnattendedFaults = await _context.FaultReport
                    .CountAsync(fr => fr.StatusFilter == "Pending"),
                TodaysRepairs = await _context.FaultReport
                    .CountAsync(fr => fr.ReportDate.Date == today),
                RecentFaults = await _context.FaultReport
                    .Include(fr => fr.Fridge)
                    .Include(fr => fr.Fridge.Customer)
                    .Where(fr => fr.StatusFilter != "Resolved" && fr.StatusFilter != "Cancelled")
                    .OrderByDescending(fr => fr.ReportDate)
                    .Take(5)
                    .ToListAsync(),
                CompletedThisWeek = await _context.FaultReport
                    .CountAsync(fr => fr.StatusFilter == "Resolved" && fr.ReportDate >= startOfWeek)
            };

            return View(dashboardData);
        }

        // GET: Faults/Process/5 - Process Fault Report
        [HttpGet]

        public async Task<IActionResult> Process(int? id)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            if (id == null)
            {
                return NotFound();
            }

            var faultReport = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .Include(fr => fr.Fridge.Customer)
                .Include(fr => fr.RepairSchedules) // Direct repair schedules
                .FirstOrDefaultAsync(fr => fr.FaultReportId == id);

            if (faultReport == null)
            {
                return NotFound();
            }

            // Get latest repair schedule or create new one
            var repairSchedule = faultReport.RepairSchedules
                .OrderByDescending(rs => rs.CreatedDate)
                .FirstOrDefault() ?? new RepairSchedule
                {
                    FaultReportId = faultReport.FaultReportId, // Link directly to FaultReport
                    FridgeId = faultReport.FridgeId,
                    Status = "Diagnosing"
                };

            ViewBag.FaultReport = faultReport;
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
            }, "Value", "Text", faultReport.Fridge?.Condition ?? "Working");

            return View(repairSchedule);
        }

        // POST: Faults/Process/5 - Process Fault Report
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(int id, [Bind("RepairID,Diagnosis,RepairNotes,Status,RepairDate,FaultReportId,FridgeID")] RepairSchedule repairSchedule, string fridgeCondition)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            if (id != repairSchedule.FaultReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // First, get the fault report to ensure we have a valid FridgeId
                    var faultReport = await _context.FaultReport
                        .Include(fr => fr.Fridge)
                        .FirstOrDefaultAsync(fr => fr.FaultReportId == id);

                    if (faultReport == null)
                    {
                        return NotFound();
                    }

                    // Update or create repair schedule
                    var existingRepair = await _context.RepairSchedules
                        .FirstOrDefaultAsync(r => r.FaultReportId == id && r.RepairID == repairSchedule.RepairID);

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
                        // Use the FridgeId from the fault report, not from the form
                        repairSchedule.FaultReportId = id;
                        repairSchedule.FridgeId = faultReport.FridgeId; // CRITICAL FIX: Use the valid FridgeId
                        repairSchedule.CreatedDate = DateTime.Now;
                        repairSchedule.UpdatedDate = DateTime.Now;
                        _context.Add(repairSchedule);
                    }

                    // Update fault report status based on repair status
                    faultReport.StatusFilter = repairSchedule.Status == "Completed" ? "Resolved" : "In Progress";
                    _context.Update(faultReport);

                    // Update fridge condition
                    if (faultReport.Fridge != null && !string.IsNullOrEmpty(fridgeCondition))
                    {
                        faultReport.Fridge.Condition = fridgeCondition;
                        faultReport.Fridge.UpdatedDate = DateTime.Now;
                        _context.Update(faultReport.Fridge);
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Fault processing updated successfully!";

                    if (repairSchedule.Status == "Completed")
                    {
                        return RedirectToAction("Complete", new { id = repairSchedule.RepairID });
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Log the detailed error
                    _logger.LogError(ex, "Database update error for fault report {FaultReportId}", id);

                    // Add a user-friendly error message
                    ModelState.AddModelError("", "Error saving changes. Please check the data and try again.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error processing fault report {FaultReportId}", id);
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            // Repopulate view data if validation fails
            var faultReportInfo = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .FirstOrDefaultAsync(fr => fr.FaultReportId == id);

            ViewBag.FaultReport = faultReportInfo;
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
        [HttpGet]
        [Route("Faults/Repair/{id?}")]
        public async Task<IActionResult> Repair(int? id)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            if (id == null)
            {
                return NotFound();
            }

            var faultReport = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .Include(fr => fr.Fridge.Customer)
                .Include(fr => fr.RepairSchedules)
                .FirstOrDefaultAsync(fr => fr.FaultReportId == id);

            if (faultReport == null)
            {
                return NotFound();
            }

            // Get latest repair schedule or create new one
            var repairSchedule = faultReport.RepairSchedules
                .OrderByDescending(rs => rs.CreatedDate)
                .FirstOrDefault() ?? new RepairSchedule
                {
                    FaultReportId = faultReport.FaultReportId,
                    FridgeId = faultReport.FridgeId,
                    Status = "Repairing"
                };

            ViewBag.FaultReport = faultReport;
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
        public async Task<IActionResult> Repair(int id, [Bind("RepairID,RepairType,PartsUsed,RepairNotes,RepairCost,Status,FaultReportId,FridgeID")] RepairSchedule repairSchedule)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            if (id != repairSchedule.FaultReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRepair = await _context.RepairSchedules
                        .FirstOrDefaultAsync(r => r.FaultReportId == id && r.RepairID == repairSchedule.RepairID);

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
                        repairSchedule.FaultReportId = id;
                        repairSchedule.Status = "Testing";
                        repairSchedule.CreatedDate = DateTime.Now;
                        repairSchedule.UpdatedDate = DateTime.Now;
                        _context.Add(repairSchedule);
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

            var faultReport = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .FirstOrDefaultAsync(fr => fr.FaultReportId == id);

            ViewBag.FaultReport = faultReport;
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
        [HttpGet]
        [Route("Faults/UpdateCondition/{id?}")]
        public async Task<IActionResult> UpdateCondition(int? id)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            if (id == null)
            {
                return NotFound();
            }

            var faultReport = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .Include(fr => fr.Fridge.Customer)
                .FirstOrDefaultAsync(fr => fr.FaultReportId == id);

            if (faultReport == null || faultReport.Fridge == null)
            {
                return NotFound();
            }

            ViewBag.FaultReport = faultReport;
            ViewBag.ConditionOptions = new SelectList(new[]
            {
                new { Value = "Working", Text = "Working - Fully Functional" },
                new { Value = "Under Repair", Text = "Under Repair" },
                new { Value = "Faulty", Text = "Faulty - Needs Attention" },
                new { Value = "Scrapped", Text = "Scrapped - Beyond Repair" }
            }, "Value", "Text", faultReport.Fridge.Condition);

            return View(faultReport.Fridge);
        }

        // POST: Faults/UpdateCondition/5 - Update Fridge Condition
        // POST: Faults/UpdateCondition/5 - Update Fridge Condition
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCondition(int id, [Bind("FridgeID,Model,SerialNumber,Brand,Type,Condition,Notes")] Fridge fridge)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

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
                        existingFridge.UpdatedDate = DateTime.Now;
                        _context.Update(existingFridge);

                        // If condition is set to "Working", mark related fault reports as resolved
                        if (fridge.Condition == "Working")
                        {
                            var relatedFaultReports = await _context.FaultReport
                                .Where(fr => fr.FridgeId == id && fr.StatusFilter != "Resolved")
                                .ToListAsync();

                            foreach (var faultReport in relatedFaultReports)
                            {
                                faultReport.StatusFilter = "Resolved";
                                _context.Update(faultReport);
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

            // ✅ FIXED: Renamed variable to avoid naming conflict
            var faultReportInfo = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .FirstOrDefaultAsync(fr => fr.FridgeId == id);

            ViewBag.FaultReport = faultReportInfo;
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
        [HttpGet]
        [Route("Faults/Complete/{id?}")]
        public async Task<IActionResult> Complete(int? id)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            if (id == null)
            {
                return NotFound();
            }

            var repairSchedule = await _context.RepairSchedules
                .Include(r => r.FaultReport)
                    .ThenInclude(fr => fr.Fridge)
                .Include(r => r.FaultReport)
                    .ThenInclude(fr => fr.Fridge.Customer)
                .FirstOrDefaultAsync(r => r.RepairID == id);

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
            ViewData["Sidebar"] = "FaultTechSubsystem";

            var repairSchedule = await _context.RepairSchedules
                .Include(r => r.FaultReport)
                .Include(r => r.Fridge)
                .FirstOrDefaultAsync(r => r.RepairID == id);

            if (repairSchedule != null)
            {
                repairSchedule.Status = "Completed";
                repairSchedule.UpdatedDate = DateTime.Now;

                // Update fault report status
                if (repairSchedule.FaultReport != null)
                {
                    repairSchedule.FaultReport.StatusFilter = "Resolved";
                    _context.Update(repairSchedule.FaultReport);
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
        [HttpGet]
        [Route("Faults/Details/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            if (id == null)
            {
                return NotFound();
            }

            var faultReport = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .Include(fr => fr.Fridge.Customer)
                .Include(fr => fr.RepairSchedules) // Direct repair schedules
                .Include(fr => fr.MaintenanceVisit)
                .FirstOrDefaultAsync(fr => fr.FaultReportId == id);

            if (faultReport == null)
            {
                return NotFound();
            }

            return View(faultReport);
        }

        [HttpGet]
        [Route("Faults/Reports")]
        public async Task<IActionResult> Reports(int? month, int? year)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            var selectedMonth = month ?? DateTime.Now.Month;
            var selectedYear = year ?? DateTime.Now.Year;
            var startDate = new DateTime(selectedYear, selectedMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var reportData = new DashboardViewModel();

            // Get basic stats using FaultReport
            reportData.TotalFaults = await _context.FaultReport
                .CountAsync(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate);

            reportData.HighPriorityFaults = await _context.FaultReport
                .CountAsync(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate &&
                                 (fr.UrgencyLevel == UrgencyLevel.High ||
                                  fr.UrgencyLevel == UrgencyLevel.Critical ||
                                  fr.UrgencyLevel == UrgencyLevel.Emergency));

            reportData.UnattendedFaults = await _context.FaultReport
                .CountAsync(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate && fr.StatusFilter == "Pending");

            // Get recent fault reports for the period
            reportData.RecentFaults = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .Include(fr => fr.Fridge.Customer)
                .Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate)
                .OrderByDescending(fr => fr.ReportDate)
                .Take(10)
                .ToListAsync();

            // Additional report data
            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.SelectedYear = selectedYear;
            ViewBag.MonthName = new DateTime(selectedYear, selectedMonth, 1).ToString("MMMM yyyy");

            // Top brands with faults
            var topBrands = await _context.FaultReport
                .Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate)
                .Include(fr => fr.Fridge)
                .GroupBy(fr => fr.Fridge.Brand)
                .Select(g => new { Brand = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();
            ViewBag.TopBrands = topBrands;

            // Common fault types
            var commonFaults = await _context.FaultReport
                .Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate)
                .GroupBy(fr => fr.FaultType)
                .Select(g => new { FaultType = g.Key, Count = g.Count() })
                .ToListAsync();
            ViewBag.CommonFaults = commonFaults;

            // Repair statistics
            var repairStats = await _context.RepairSchedules
                .Where(r => r.CreatedDate >= startDate && r.CreatedDate <= endDate)
                .GroupBy(r => r.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();
            ViewBag.RepairStats = repairStats;

            return View(reportData);
        }

        private bool FaultReportExists(int id)
        {
            return _context.FaultReport.Any(e => e.FaultReportId == id);
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
        public List<FaultReport> RecentFaults { get; set; } = new List<FaultReport>();
    }
}