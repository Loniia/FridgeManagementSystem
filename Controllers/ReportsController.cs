using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace FridgeManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly FridgeDbContext _context;

        public ReportsController(FridgeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetMaintenanceChartData()
        {
            try
            {
                var sixMonthsAgo = DateTime.Now.AddMonths(-6);

                var monthlyData = await _context.MaintenanceVisit
                    .Include(v => v.MaintenanceRequest)
                    .Where(v => v.ScheduledDate >= sixMonthsAgo)
                    .GroupBy(v => new { v.ScheduledDate.Year, v.ScheduledDate.Month })
                    .Select(g => new
                    {
                        Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                        Scheduled = g.Count(),
                        Completed = g.Count(v => v.Status == Models.TaskStatus.Complete),
                        Rescheduled = g.Count(v => v.Status == Models.TaskStatus.Rescheduled),
                        Canceled = g.Count(v => v.Status == Models.TaskStatus.Cancelled),
                        AvgResponseTime = g.Where(v => v.Status == Models.TaskStatus.Complete &&
                                                     v.MaintenanceRequest.CompletedDate != null)
                                         .Average(v => (double?)EF.Functions.DateDiffDay(
                                             v.MaintenanceRequest.RequestDate,
                                             v.MaintenanceRequest.CompletedDate.Value)) ?? 0
                    })
                    .OrderBy(x => x.Month)
                    .ToListAsync();

                return Json(monthlyData);
            }
            catch (Exception ex)
            {
                // Log error
                return Json(new { error = "Failed to load chart data" });
            }
        }
    }
}