
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json; // for serialization
namespace FridgeManagementSystem.Areas.MaintenanceSubSystem.Controllers
{
    [Area("MaintenanceSubSystem")]
    public class MaintenanceHomeController : Controller
    {
        private readonly ILogger<MaintenanceHomeController> _logger;
        private readonly FridgeDbContext _context;

        public MaintenanceHomeController(FridgeDbContext context, ILogger<MaintenanceHomeController> logger)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var model = new MaintenanceDashboardViewModel();

            // ✅ Counts for category chart
            model.PendingRequests = await _context.MaintenanceRequest.CountAsync(r => r.IsActive && r.TaskStatus == Models.TaskStatus.Pending);
            model.ScheduledVisits = await _context.MaintenanceRequest.CountAsync(r => r.IsActive && r.TaskStatus == Models.TaskStatus.Scheduled);
            model.RescheduledVisits = await _context.MaintenanceRequest.CountAsync(r => r.IsActive && r.TaskStatus == Models.TaskStatus.Rescheduled);
            model.InProgressVisits = await _context.MaintenanceRequest.CountAsync(r => r.IsActive && r.TaskStatus == Models.TaskStatus.InProgress);
            model.CompletedTasks = await _context.MaintenanceVisit
    .CountAsync(v => v.Status == Models.TaskStatus.Complete && v.MaintenanceRequest.IsActive);

            model.CancelledVisits = await _context.MaintenanceRequest.CountAsync(r => r.IsActive && r.TaskStatus == Models.TaskStatus.Cancelled);

            // ✅ Labels + values for chart
            model.CategoryLabels = new List<string> { "Pending", "Scheduled", "Rescheduled", "InProgress", "Completed", "Cancelled" };
            model.CategoryValues = new List<int> { model.PendingRequests, model.ScheduledVisits, model.RescheduledVisits, model.InProgressVisits, model.CompletedTasks, model.CancelledVisits };

            // ✅ Last 6 months chart
            model.MonthLabels = Enumerable.Range(0, 6).Select(i => DateTime.Now.AddMonths(-i).ToString("MMM")).Reverse().ToList();
            model.CompletedValues = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var month = DateTime.Now.AddMonths(-i);
                    return _context.MaintenanceVisit.Count(v =>
                        v.Status == Models.TaskStatus.Complete &&
                        v.MaintenanceRequest.CompletedDate.HasValue &&
                        v.MaintenanceRequest.CompletedDate.Value.Month == month.Month &&
                        v.MaintenanceRequest.CompletedDate.Value.Year == month.Year);
                })
                .Reverse()
                .ToList();

            // ✅ Populate table
            var pendingRequests = await _context.MaintenanceRequest
                .Include(r => r.Fridge)
                    .ThenInclude(f => f.Customer)
                        .ThenInclude(c => c.Location)
                .Where(r => r.IsActive && r.TaskStatus == Models.TaskStatus.Pending)
                .OrderByDescending(r => r.RequestDate)
                .Take(15)
                .ToListAsync();

            model.PendingRequestsNeedingScheduling = pendingRequests.Select(r => new MaintenanceRequestSummary
            {
                MaintenanceRequestId = r.MaintenanceRequestId,
                RequestDate = r.RequestDate ?? DateTime.MinValue, // handle null safely
                FridgeBrand = r.Fridge?.Brand ?? "Unknown",
                FridgeModel = r.Fridge?.Model ?? "Unknown",
                CustomerName = r.Fridge?.Customer?.FullName ?? "Unknown",
                CustomerAddress = r.Fridge?.Customer?.Location?.Address ?? "Address not available",
                DaysPending = (int)((DateTime.Now - (r.RequestDate ?? DateTime.Now)).TotalDays) // handle null safely
            }).ToList();

            return View(model);
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

