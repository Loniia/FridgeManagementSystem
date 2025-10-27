
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


        public IActionResult Index()
        {
            var model = new MaintenanceDashboardViewModel();
            var requests = _context.MaintenanceRequest
                .Include(r => r.Fridge) // include fridge for customer info
                .AsQueryable();

            // Counts by status
            model.PendingRequests = requests.Count(r => r.TaskStatus == Models.TaskStatus.Pending && r.IsActive);
            model.ScheduledVisits = requests.Count(r => r.TaskStatus == Models.TaskStatus.Scheduled && r.IsActive);
            model.RescheduledVisits = requests.Count(r => r.TaskStatus == Models.TaskStatus.Rescheduled && r.IsActive);
            model.InProgressVisits = requests.Count(r => r.TaskStatus == Models.TaskStatus.InProgress && r.IsActive);
            model.CompletedTasks = requests.Count(r => r.TaskStatus == Models.TaskStatus.Complete);
            model.CancelledVisits = requests.Count(r => r.TaskStatus == Models.TaskStatus.Cancelled && r.IsActive);

            // Completion %
            var total = (double)(model.PendingRequests + model.ScheduledVisits + model.RescheduledVisits +
                                 model.InProgressVisits + model.CompletedTasks + model.CancelledVisits);
            model.CompletionPercent = total > 0 ? (int)Math.Round(100.0 * model.CompletedTasks / total) : 0;

            // Category chart
            model.CategoryLabels = new List<string> { "Pending", "Scheduled", "Rescheduled", "InProgress", "Completed", "Cancelled" };
            model.CategoryValues = new List<int> {
        model.PendingRequests,
        model.ScheduledVisits,
        model.RescheduledVisits,
        model.InProgressVisits,
        model.CompletedTasks,
        model.CancelledVisits
    };

            // 30-day trend
            int days = 30;
            var today = DateTime.Today;
            var start = today.AddDays(-days + 1);

            var completedInRange = _context.MaintenanceRequest
        .Where(r => r.TaskStatus == Models.TaskStatus.Complete &&
                    r.CompletedDate != null &&
                    r.CompletedDate.Value.Date >= start &&
                    r.CompletedDate.Value.Date <= today)
        .Select(r => r.CompletedDate.Value.Date)
        .ToList();
            model.TrendLabels = new List<string>();
            model.TrendCompletedCounts = new List<int>();
            for (int i = 0; i < days; i++)
            {
                var day = start.AddDays(i);
                model.TrendLabels.Add(day.ToString("MMM d"));
                model.TrendCompletedCounts.Add(completedInRange.Count(d => d.Date == day));
            }

            // Today’s completed visits
            var startOfToday = today;
            var endOfToday = today.AddDays(1);

            model.TodayVisits = _context.MaintenanceRequest
    .Include(r => r.Fridge)
    .Where(r => r.TaskStatus == Models.TaskStatus.Complete &&
                r.CompletedDate >= startOfToday &&
                r.CompletedDate < endOfToday &&
                r.IsActive)
    .OrderBy(r => r.CompletedDate)
    .ToList();


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

