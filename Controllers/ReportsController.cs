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

        //============
        //Customer Management SubSystem Report Part
        //============
        // ✅ Customer Reports - Summary Data
        [HttpGet]
        public async Task<IActionResult> GetCustomerSummaryData()
        {
            var summary = new
            {
                TotalCustomers = await _context.Customers.CountAsync(),
                ActiveCustomers = await _context.Customers.CountAsync(c => c.IsActive),
                TotalAllocated = await _context.FridgeAllocation
                    .CountAsync(a => a.Status == "Allocated"),
                AvailableFridges = await _context.Fridge
                    .CountAsync(f => f.Status == "Available" && f.IsActive)
            };

            return Json(summary);
        }

        // ✅ Customer Status Distribution
        [HttpGet]
        public async Task<IActionResult> GetCustomerStatusData()
        {
            var statusData = await _context.Customers
                .GroupBy(c => c.IsActive)
                .Select(g => new
                {
                    Status = g.Key ? "Active" : "Inactive",
                    Count = g.Count()
                })
                .ToListAsync();

            return Json(statusData);
        }

        // ✅ Location Distribution
        [HttpGet]
        public async Task<IActionResult> GetLocationDistributionData()
        {
            var locationData = await _context.Customers
                .Include(c => c.Location)
                .Where(c => c.Location != null)
                .GroupBy(c => c.Location.Address)
                .Select(g => new
                {
                    Location = g.Key,
                    CustomerCount = g.Count(),
                    ActiveAllocations = g.SelectMany(c => c.FridgeAllocation)
                                       .Count(a => a.Status == "Allocated")
                })
                .OrderByDescending(x => x.CustomerCount)
                .Take(10)
                .ToListAsync();

            return Json(locationData);
        }

        // ✅ Fridge Allocation Summary
        [HttpGet]
        public async Task<IActionResult> GetFridgeAllocationSummary()
        {
            var allocationSummary = await _context.FridgeAllocation
                .GroupBy(a => a.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // Add total count
            var total = allocationSummary.Sum(x => x.Count);
            var result = allocationSummary.Select(x => new
            {
                x.Status,
                x.Count,
                Percentage = total > 0 ? Math.Round((x.Count / (double)total) * 100, 1) : 0
            }).ToList();

            return Json(result);
        }

        // ✅ Allocation Trends
        [HttpGet]
        public async Task<IActionResult> GetAllocationTrendsData()
        {
            var sixMonthsAgo = DateTime.Today.AddMonths(-6);

            var trends = await _context.FridgeAllocation
                .Where(a => a.AllocationDate >= DateOnly.FromDateTime(sixMonthsAgo))
                .GroupBy(a => new { Year = a.AllocationDate.Year, Month = a.AllocationDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Allocations = g.Count(a => a.Status == "Allocated"),
                    Returns = g.Count(a => a.Status == "Returned"),
                    Scrapped = g.Count(a => a.Status == "Scrapped")
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            var result = trends.Select(t => new
            {
                Month = new DateTime(t.Year, t.Month, 1).ToString("MMM yyyy"),
                t.Allocations,
                t.Returns,
                t.Scrapped
            }).ToList();

            return Json(result);
        }

        // ✅ Top Customers
        [HttpGet]
        public async Task<IActionResult> GetTopCustomersData()
        {
            var topCustomers = await _context.Customers
                .Select(c => new
                {
                    CustomerName = c.FullName,
                    TotalAllocations = c.FridgeAllocation.Count(a => a.Status == "Allocated"),
                    ActiveAllocations = c.FridgeAllocation.Count(a => a.Status == "Allocated"),
                    ReturnedAllocations = c.FridgeAllocation.Count(a => a.Status == "Returned")
                })
                .Where(c => c.TotalAllocations > 0)
                .OrderByDescending(c => c.ActiveAllocations)
                .Take(8)
                .ToListAsync();

            return Json(topCustomers);
        }
    }
}