using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public FaultsController(FridgeDbContext context, ILogger<FaultsController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Faults - View Fault Reports
        public async Task<IActionResult> Index()
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            var faultReports = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .Include(fr => fr.Fridge.Customer)
                .Include(fr => fr.RepairSchedules)
                .Where(fr => fr.Status != FridgeManagementSystem.Models.TaskStatus.Complete &&
                            fr.Status != FridgeManagementSystem.Models.TaskStatus.Cancelled)
                .OrderByDescending(fr => fr.UrgencyLevel)
                .ThenByDescending(fr => fr.ReportDate)
                .ToListAsync();

            return View(faultReports);
        }

        // GET: Faults/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            // Remove automatic date filtering or make it optional
            var dashboardData = new DashboardViewModel
            {
                TotalFaults = await _context.FaultReport
                    .CountAsync(fr => fr.Status != FridgeManagementSystem.Models.TaskStatus.Complete &&
                                    fr.Status != FridgeManagementSystem.Models.TaskStatus.Cancelled),
                HighPriorityFaults = await _context.FaultReport
                    .CountAsync(fr => (fr.UrgencyLevel == UrgencyLevel.High ||
                                     fr.UrgencyLevel == UrgencyLevel.Critical ||
                                     fr.UrgencyLevel == UrgencyLevel.Emergency) &&
                                     fr.Status != FridgeManagementSystem.Models.TaskStatus.Complete &&
                                     fr.Status != FridgeManagementSystem.Models.TaskStatus.Cancelled),
                UnattendedFaults = await _context.FaultReport
                    .CountAsync(fr => fr.Status == FridgeManagementSystem.Models.TaskStatus.Pending),
                // Remove TodaysRepairs or make it configurable
                TodaysRepairs = 0, // Optional: remove this property
                RecentFaults = await _context.FaultReport
                    .Include(fr => fr.Fridge)
                    .Include(fr => fr.Fridge.Customer)
                    .Where(fr => fr.Status != FridgeManagementSystem.Models.TaskStatus.Complete &&
                                fr.Status != FridgeManagementSystem.Models.TaskStatus.Cancelled)
                    .OrderByDescending(fr => fr.ReportDate)
                    .Take(5)
                    .ToListAsync(),
                // Remove CompletedThisWeek or make it configurable
                CompletedThisWeek = 0 // Optional: remove this property
            };

            return View(dashboardData);
        }

        // GET: Faults/Create - Create New Fault Report
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            // Populate dropdowns
            ViewBag.FridgeId = new SelectList(_context.Fridge, "FridgeId", "SerialNumber");
            ViewBag.FaultType = new SelectList(Enum.GetValues(typeof(FaultType)));
            ViewBag.UrgencyLevel = new SelectList(Enum.GetValues(typeof(UrgencyLevel)));

            return View();
        }

        // POST: Faults/Create - Create New Fault Report
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FaultReportId,ReportDate,FaultType,UrgencyLevel,FaultDescription,FridgeId")] FaultReport faultReport)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            if (ModelState.IsValid)
            {
                try
                {
                    // Set default status - NO automatic date setting
                    faultReport.Status = FridgeManagementSystem.Models.TaskStatus.Pending;
                    faultReport.StatusFilter = FridgeManagementSystem.Models.TaskStatus.Pending.ToString();

                    _context.Add(faultReport);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Fault report created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating fault report");
                    ModelState.AddModelError("", "Error creating fault report. Please try again.");
                }
            }

            // Repopulate dropdowns if validation fails
            ViewBag.FridgeId = new SelectList(_context.Fridge, "FridgeId", "SerialNumber", faultReport.FridgeId);
            ViewBag.FaultType = new SelectList(Enum.GetValues(typeof(FaultType)), faultReport.FaultType);
            ViewBag.UrgencyLevel = new SelectList(Enum.GetValues(typeof(UrgencyLevel)), faultReport.UrgencyLevel);

            return View(faultReport);
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
                    Status = "Diagnosing"
                };

            ViewBag.FaultReport = faultReport;

            ViewBag.StatusOptions = new SelectList(new[]
            {
                new { Value = FridgeManagementSystem.Models.TaskStatus.Pending, Text = "Pending" },
                new { Value = FridgeManagementSystem.Models.TaskStatus.InProgress, Text = "In Progress" },
                new { Value = FridgeManagementSystem.Models.TaskStatus.OnHold, Text = "On Hold" },
                new { Value = FridgeManagementSystem.Models.TaskStatus.Complete, Text = "Complete" },
                new { Value = FridgeManagementSystem.Models.TaskStatus.Cancelled, Text = "Cancelled" }
            }, "Value", "Text", faultReport.Status);

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
        public async Task<IActionResult> Process(int id, [Bind("RepairID,Diagnosis,RepairNotes,Status,RepairDate,FaultReportId,FridgeID,CreatedDate,UpdatedDate")] RepairSchedule repairSchedule, FridgeManagementSystem.Models.TaskStatus faultStatus, string fridgeCondition)
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
                        existingRepair.RepairDate = repairSchedule.RepairDate;
                        // Don't automatically update UpdatedDate - use the provided value
                        if (repairSchedule.UpdatedDate == default)
                        {
                            existingRepair.UpdatedDate = DateTime.Now; // Only if not provided
                        }
                        else
                        {
                            existingRepair.UpdatedDate = repairSchedule.UpdatedDate;
                        }
                        _context.Update(existingRepair);
                    }
                    else
                    {
                        repairSchedule.FaultReportId = id;
                        repairSchedule.FridgeId = faultReport.FridgeId;
                        // Use provided dates or set defaults only if not provided
                        if (repairSchedule.CreatedDate == default)
                        {
                            repairSchedule.CreatedDate = DateTime.Now;
                        }
                        if (repairSchedule.UpdatedDate == default)
                        {
                            repairSchedule.UpdatedDate = DateTime.Now;
                        }
                        _context.Add(repairSchedule);
                    }

                    // Update fault report status
                    faultReport.Status = faultStatus;
                    faultReport.StatusFilter = faultStatus.ToString();
                    _context.Update(faultReport);

                    // Update fridge condition
                    if (faultReport.Fridge != null && !string.IsNullOrEmpty(fridgeCondition))
                    {
                        faultReport.Fridge.Condition = fridgeCondition;
                        // Don't automatically update fridge date
                        _context.Update(faultReport.Fridge);
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Fault processing updated successfully!";

                    if (faultStatus == FridgeManagementSystem.Models.TaskStatus.Complete)
                    {
                        return RedirectToAction("Complete", new { id = repairSchedule.RepairID });
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Database update error for fault report {FaultReportId}", id);
                    ModelState.AddModelError("", "Error saving changes. Please check the data and try again.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error processing fault report {FaultReportId}", id);
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            var faultReportInfo = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .FirstOrDefaultAsync(fr => fr.FaultReportId == id);

            ViewBag.FaultReport = faultReportInfo;
            ViewBag.StatusOptions = new SelectList(new[]
            {
                new { Value = FridgeManagementSystem.Models.TaskStatus.Pending, Text = "Pending" },
                new { Value = FridgeManagementSystem.Models.TaskStatus.InProgress, Text = "In Progress" },
                new { Value = FridgeManagementSystem.Models.TaskStatus.OnHold, Text = "On Hold" },
                new { Value = FridgeManagementSystem.Models.TaskStatus.Complete, Text = "Complete" },
                new { Value = FridgeManagementSystem.Models.TaskStatus.Cancelled, Text = "Cancelled" }
            }, "Value", "Text", faultStatus);

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

            // Don't automatically update status - let user control it
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
        public async Task<IActionResult> Repair(int id, [Bind("RepairID,RepairType,PartsUsed,RepairNotes,RepairCost,Status,FaultReportId,FridgeID,CreatedDate,UpdatedDate")] RepairSchedule repairSchedule)
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
                        existingRepair.Status = repairSchedule.Status;
                        // Use provided date or default
                        if (repairSchedule.UpdatedDate == default)
                        {
                            existingRepair.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            existingRepair.UpdatedDate = repairSchedule.UpdatedDate;
                        }
                        _context.Update(existingRepair);
                    }
                    else
                    {
                        repairSchedule.FaultReportId = id;
                        // Use provided dates or defaults
                        if (repairSchedule.CreatedDate == default)
                        {
                            repairSchedule.CreatedDate = DateTime.Now;
                        }
                        if (repairSchedule.UpdatedDate == default)
                        {
                            repairSchedule.UpdatedDate = DateTime.Now;
                        }
                        _context.Add(repairSchedule);
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Repair details saved successfully!";
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

        // POST: Faults/UpdateCondition/5 - Update Fridge Condition
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCondition(int id, [Bind("FridgeId,Model,SerialNumber,Brand,FridgeType,Condition,Notes,UpdatedDate")] Fridge fridge)
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
                        existingFridge.Notes = fridge.Notes;

                        // Use provided date or current time if null/default
                        if (fridge.UpdatedDate == null || fridge.UpdatedDate == default(DateTime))
                        {
                            existingFridge.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            existingFridge.UpdatedDate = fridge.UpdatedDate;
                        }

                        _context.Update(existingFridge);

                        // Auto-complete fault reports when condition set to "Working"
                        if (fridge.Condition == "Working")
                        {
                            var relatedFaultReports = await _context.FaultReport
                                .Where(fr => fr.FridgeId == id && fr.Status != FridgeManagementSystem.Models.TaskStatus.Complete)
                                .ToListAsync();

                            foreach (var faultReport in relatedFaultReports)
                            {
                                faultReport.Status = FridgeManagementSystem.Models.TaskStatus.Complete;
                                faultReport.StatusFilter = FridgeManagementSystem.Models.TaskStatus.Complete.ToString();
                                _context.Update(faultReport);
                            }
                        }

                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = $"Fridge condition updated to '{fridge.Condition}' successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Fridge not found.";
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
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating fridge condition for fridge {FridgeId}", id);
                    TempData["ErrorMessage"] = "An error occurred while updating the fridge condition.";
                }
            }

            // If we get here, there were validation errors
            var faultReportInfo = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .Include(fr => fr.Fridge.Customer)
                .Include(fr => fr.Fridge.Location)
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

        // POST: Faults/Complete/5 - Complete Repair Process
        [HttpPost, ActionName("Complete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteConfirmed(int id, DateTime? completionDate)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            var repairSchedule = await _context.RepairSchedules
                .Include(r => r.FaultReport)
                .Include(r => r.Fridge)
                .FirstOrDefaultAsync(r => r.RepairID == id);

            if (repairSchedule != null)
            {
                repairSchedule.Status = "Completed";
                // Use provided completion date or current date
                repairSchedule.UpdatedDate = completionDate ?? DateTime.Now;

                if (repairSchedule.FaultReport != null)
                {
                    repairSchedule.FaultReport.Status = FridgeManagementSystem.Models.TaskStatus.Complete;
                    repairSchedule.FaultReport.StatusFilter = FridgeManagementSystem.Models.TaskStatus.Complete.ToString();
                    _context.Update(repairSchedule.FaultReport);
                }

                if (repairSchedule.Fridge != null)
                {
                    repairSchedule.Fridge.Condition = "Working";
                    repairSchedule.Fridge.UpdatedDate = completionDate ?? DateTime.Now;
                    _context.Update(repairSchedule.Fridge);
                }

                _context.Update(repairSchedule);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Repair completed successfully! Fridge is now working.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Faults/Reports - Updated to allow date range selection
        [HttpGet]
        [Route("Faults/Reports")]
        public async Task<IActionResult> Reports(DateTime? startDate, DateTime? endDate)
        {
            ViewData["Sidebar"] = "FaultTechSubsystem";

            // Default to last 30 days if no dates provided
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var reportData = new DashboardViewModel();

            reportData.TotalFaults = await _context.FaultReport
                .CountAsync(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate);

            reportData.HighPriorityFaults = await _context.FaultReport
                .CountAsync(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate &&
                                 (fr.UrgencyLevel == UrgencyLevel.High ||
                                  fr.UrgencyLevel == UrgencyLevel.Critical ||
                                  fr.UrgencyLevel == UrgencyLevel.Emergency));

            reportData.UnattendedFaults = await _context.FaultReport
                .CountAsync(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate &&
                                 fr.Status == FridgeManagementSystem.Models.TaskStatus.Pending);

            reportData.RecentFaults = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .Include(fr => fr.Fridge.Customer)
                .Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate)
                .OrderByDescending(fr => fr.ReportDate)
                .Take(10)
                .ToListAsync();

            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");

            // ... rest of report data population

            return View(reportData);
        }

        // Helper method to update fault status
        private async Task UpdateFaultStatus(int faultReportId, FridgeManagementSystem.Models.TaskStatus newStatus)
        {
            var faultReport = await _context.FaultReport.FindAsync(faultReportId);
            if (faultReport != null)
            {
                faultReport.Status = newStatus;
                faultReport.StatusFilter = newStatus.ToString();
                _context.Update(faultReport);
                await _context.SaveChangesAsync();
            }
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