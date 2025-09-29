
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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



        // HomePage 
        public IActionResult Index()
        {
            // Count everything from MaintenanceRequest for consistency
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Requests still pending
            ViewBag.PendingRequests = _context.MaintenanceRequest
                .Count(r => r.TaskStatus == Models.TaskStatus.Pending && r.IsActive);

            // Visits currently scheduled (requests with Scheduled status)
            ViewBag.ScheduledVisits = _context.MaintenanceRequest
                .Count(r => r.TaskStatus == Models.TaskStatus.Scheduled && r.IsActive);

            // Visits that were rescheduled
            ViewBag.RescheduledVisits = _context.MaintenanceRequest
                .Count(r => r.TaskStatus == Models.TaskStatus.Rescheduled && r.IsActive);

            // Visits in progress
            ViewBag.InProgressVisits = _context.MaintenanceRequest
                .Count(r => r.TaskStatus == Models.TaskStatus.InProgress && r.IsActive);

            // Completed requests this month
            ViewBag.CompletedTasks = _context.MaintenanceRequest
                .Count(r => r.TaskStatus == Models.TaskStatus.Complete &&
                           r.RequestDate >= startOfMonth && r.IsActive);

            // Cancelled requests
            ViewBag.CancelledVisits = _context.MaintenanceRequest
                .Count(r => r.TaskStatus == Models.TaskStatus.Cancelled && r.IsActive);

            return View();
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

