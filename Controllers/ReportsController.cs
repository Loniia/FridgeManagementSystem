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
        public async Task <IActionResult> Index()
        {
            //var model = new ReportDashboardViewModel();

            //// ✅ Load the KPI data
            //model.MaintenanceKpi = await GetMonthlyMaintenanceKpi();
            return View();
        }

        public  IActionResult FaultReports()
        {
            
            return View();
        }
    //    [HttpGet]
    //    private async Task<MaintenanceKpiViewModel> GetMonthlyMaintenanceKpi()
    //    {
    //        var now = DateTime.Now;
    //        var month = now.Month;
    //        var year = now.Year;

    //        var kpi = new MaintenanceKpiViewModel();

    //        //// Requests created this month
    //        //var monthlyRequests = await _context.MaintenanceRequest
    //        //    .Where(r => r.RequestDate.Month == month && r.RequestDate.Year == year)
    //        //    .ToListAsync();

    //        var monthlyCompleted = monthlyRequests
    //.Where(r => r.TaskStatus == Models.TaskStatus.Complete && r.CompletedDate != null)
    //.ToList();

    //        kpi.CompletedThisMonth = monthlyCompleted.Count;

    //        // Avg days to complete
    //        kpi.AvgCompletionDays = monthlyCompleted.Any()
    //            ? monthlyCompleted.Average(r => (r.CompletedDate.Value - r.RequestDate).TotalDays)
    //            : 0;

    //        // Success rate
    //        kpi.SuccessRate = monthlyRequests.Any()
    //            ? (monthlyCompleted.Count * 100.0) / monthlyRequests.Count
    //            : 0;

    //        // Repeat visits: >1 completed visit for same fridge this month
    //        kpi.MonthlyRepeatVisits = await _context.MaintenanceVisit
    //            .Where(v => v.Status == Models.TaskStatus.Complete &&
    //                        v.ScheduledDate.Month == month &&
    //                        v.ScheduledDate.Year == year)
    //            .GroupBy(v => v.FridgeId)
    //            .Where(g => g.Count() > 1)
    //            .CountAsync();

    //        return kpi;
    //    }


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
        [HttpGet]
        public async Task<IActionResult> GetAllocationTrendsData()
        {
            var sixMonthsAgo = DateTime.Today.AddMonths(-6);
            var sixMonthsAgoDateOnly = DateOnly.FromDateTime(sixMonthsAgo);

            var trends = await _context.FridgeAllocation
                .Where(a => a.AllocationDate != null && a.AllocationDate.Value >= sixMonthsAgoDateOnly)
                .GroupBy(a => new
                {
                    Year = a.AllocationDate.Value.Year,
                    Month = a.AllocationDate.Value.Month
                })
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


        // ============
        // FAULT MANAGEMENT REPORTS WITH DATE RANGE SUPPORT
        // ============

        // ✅ Fault Summary Data with Date Range
        [HttpGet]
        public async Task<IActionResult> GetFaultSummaryData(DateTime? startDate, DateTime? endDate)
        {
            // Default to last 30 days if no dates provided
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var query = _context.FaultReport.Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate);

            var summary = new
            {
                TotalFaults = await query.CountAsync(),
                PendingFaults = await query.CountAsync(fr => fr.Status == FridgeManagementSystem.Models.TaskStatus.Pending),
                HighPriorityFaults = await query.CountAsync(fr =>
                    fr.UrgencyLevel == UrgencyLevel.High ||
                    fr.UrgencyLevel == UrgencyLevel.Critical ||
                    fr.UrgencyLevel == UrgencyLevel.Emergency),
                ResolvedThisWeek = await query.CountAsync(fr =>
                    fr.Status == FridgeManagementSystem.Models.TaskStatus.Complete &&
                    fr.ReportDate >= DateTime.Now.AddDays(-7)),
                AverageResolutionDays = await query
                    .Where(fr => fr.Status == FridgeManagementSystem.Models.TaskStatus.Complete && fr.RepairSchedules.Any())
                    .AverageAsync(fr => (double?)(fr.RepairSchedules.Max(rs => rs.UpdatedDate) - fr.ReportDate).TotalDays) ?? 0,
                DateRange = new { StartDate = startDate, EndDate = endDate }
            };

            return Json(summary);
        }

        // ✅ Fault Type Distribution with Date Range
        [HttpGet]
        public async Task<IActionResult> GetFaultTypeDistribution(DateTime? startDate, DateTime? endDate)
        {
            // Default to last 30 days if no dates provided
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var baseQuery = _context.FaultReport.Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate);
            var totalFaults = await baseQuery.CountAsync();

            var faultTypeData = await baseQuery
                .GroupBy(fr => fr.FaultType)
                .Select(g => new
                {
                    FaultType = g.Key.ToString(),
                    Count = g.Count(),
                    Percentage = totalFaults > 0 ? (double)g.Count() / totalFaults * 100 : 0
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return Json(new { Data = faultTypeData, DateRange = new { StartDate = startDate, EndDate = endDate } });
        }

        // ✅ Urgency Level Distribution with Date Range
        [HttpGet]
        public async Task<IActionResult> GetUrgencyDistribution(DateTime? startDate, DateTime? endDate)
        {
            // Default to last 30 days if no dates provided
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var query = _context.FaultReport.Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate);

            var urgencyData = await query
                .GroupBy(fr => fr.UrgencyLevel)
                .Select(g => new
                {
                    UrgencyLevel = g.Key.ToString(),
                    Count = g.Count(),
                    AverageResolutionTime = g.Where(fr => fr.Status == FridgeManagementSystem.Models.TaskStatus.Complete && fr.RepairSchedules.Any())
                                           .Average(fr => (double?)(fr.RepairSchedules.Max(rs => rs.UpdatedDate) - fr.ReportDate).TotalDays) ?? 0
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return Json(new { Data = urgencyData, DateRange = new { StartDate = startDate, EndDate = endDate } });
        }

        // ✅ Monthly Fault Trends (uses custom months parameter, not date range)
        [HttpGet]
        public async Task<IActionResult> GetFaultTrendsData(int months = 6)
        {
            var startDate = DateTime.Today.AddMonths(-months);

            var monthlyTrends = await _context.FaultReport
                .Where(fr => fr.ReportDate >= startDate)
                .GroupBy(fr => new { fr.ReportDate.Year, fr.ReportDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalFaults = g.Count(),
                    PendingFaults = g.Count(fr => fr.Status == FridgeManagementSystem.Models.TaskStatus.Pending),
                    ResolvedFaults = g.Count(fr => fr.Status == FridgeManagementSystem.Models.TaskStatus.Complete),
                    HighPriorityFaults = g.Count(fr =>
                        fr.UrgencyLevel == UrgencyLevel.High ||
                        fr.UrgencyLevel == UrgencyLevel.Critical),
                    AverageUrgency = g.Average(fr => (int)fr.UrgencyLevel)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            var result = monthlyTrends.Select(t => new
            {
                Period = new DateTime(t.Year, t.Month, 1).ToString("MMM yyyy"),
                t.TotalFaults,
                t.PendingFaults,
                t.ResolvedFaults,
                t.HighPriorityFaults,
                t.AverageUrgency
            }).ToList();

            return Json(new { Data = result, Period = $"{months} months" });
        }

        // ✅ Brand Fault Analysis with Date Range
        [HttpGet]
        public async Task<IActionResult> GetBrandFaultAnalysis(DateTime? startDate, DateTime? endDate)
        {
            // Default to last 30 days if no dates provided
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var faultQuery = _context.FaultReport
                .Include(fr => fr.Fridge)
                .Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate);

            var brandAnalysis = await faultQuery
                .GroupBy(fr => fr.Fridge.Brand)
                .Select(g => new
                {
                    Brand = g.Key,
                    TotalFaults = g.Count(),
                    MostCommonFaultType = g.GroupBy(x => x.FaultType)
                                          .OrderByDescending(ft => ft.Count())
                                          .Select(ft => ft.Key.ToString())
                                          .FirstOrDefault(),
                    FaultRate = (double)g.Count() / _context.Fridge.Count(f => f.Brand == g.Key),
                    AverageRepairCost = g.Where(fr => fr.RepairSchedules.Any())
                                        .Average(fr => fr.RepairSchedules.Sum(rs => rs.RepairCost)) ?? 0,
                    ResolutionRate = (double)g.Count(fr => fr.Status == FridgeManagementSystem.Models.TaskStatus.Complete) / g.Count() * 100
                })
                .Where(x => x.TotalFaults > 0)
                .OrderByDescending(x => x.TotalFaults)
                .Take(10)
                .ToListAsync();

            return Json(new { Data = brandAnalysis, DateRange = new { StartDate = startDate, EndDate = endDate } });
        }

        // ✅ Seasonal Fault Patterns (uses year parameter, not date range)
        [HttpGet]
        public async Task<IActionResult> GetSeasonalFaultPatterns(int year = 0)
        {
            var targetYear = year > 0 ? year : DateTime.Now.Year;
            var yearStart = new DateTime(targetYear, 1, 1);
            var yearEnd = new DateTime(targetYear, 12, 31);

            var seasonalData = await _context.FaultReport
                .Where(fr => fr.ReportDate >= yearStart && fr.ReportDate <= yearEnd)
                .GroupBy(fr => fr.ReportDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    MonthName = new DateTime(targetYear, g.Key, 1).ToString("MMM"),
                    TotalFaults = g.Count(),
                    CoolingIssues = g.Count(fr => fr.FaultType == FaultType.CoolingIssue),
                    ElectricalIssues = g.Count(fr => fr.FaultType == FaultType.Electrical),
                    DoorSealIssues = g.Count(fr => fr.FaultType == FaultType.DoorSeal),
                    MostCommonFault = g.GroupBy(fr => fr.FaultType)
                                      .OrderByDescending(ft => ft.Count())
                                      .Select(ft => ft.Key.ToString())
                                      .FirstOrDefault()
                })
                .OrderBy(x => x.Month)
                .ToListAsync();

            return Json(new { Data = seasonalData, Year = targetYear });
        }

        // ✅ Technician Performance with Date Range
        [HttpGet]
        public async Task<IActionResult> GetTechnicianPerformance(DateTime? startDate, DateTime? endDate)
        {
            // Default to last 30 days if no dates provided
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var technicianPerformance = await _context.RepairSchedules
                .Where(rs => rs.Status == "Completed" && rs.FaultReport != null &&
                             rs.FaultReport.ReportDate >= startDate && rs.FaultReport.ReportDate <= endDate)
                .Include(rs => rs.FaultReport)
                .GroupBy(rs => rs.FaultReport.MaintenanceVisit.EmployeeID)
                .Select(g => new
                {
                    TechnicianId = g.Key,
                    CompletedJobs = g.Count(),
                    AverageCompletionHours = g.Average(rs => (rs.UpdatedDate - rs.CreatedDate).TotalHours),
                    SuccessRate = (double)g.Count(rs => rs.FaultReport.Status == FridgeManagementSystem.Models.TaskStatus.Complete) / g.Count() * 100,
                    MostCommonRepairType = g.GroupBy(rs => rs.RepairType)
                                          .OrderByDescending(rt => rt.Count())
                                          .Select(rt => rt.Key)
                                          .FirstOrDefault() ?? "Various"
                })
                .OrderByDescending(x => x.CompletedJobs)
                .Take(8)
                .ToListAsync();

            return Json(new { Data = technicianPerformance, DateRange = new { StartDate = startDate, EndDate = endDate } });
        }

        // ✅ Resolution Time Analysis with Date Range
        [HttpGet]
        public async Task<IActionResult> GetResolutionTimeAnalysis(DateTime? startDate, DateTime? endDate)
        {
            // Default to last 30 days if no dates provided
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var resolutionData = await _context.FaultReport
                .Where(fr => fr.Status == FridgeManagementSystem.Models.TaskStatus.Complete &&
                             fr.RepairSchedules.Any() &&
                             fr.ReportDate >= startDate && fr.ReportDate <= endDate)
                .Select(fr => new
                {
                    FaultType = fr.FaultType.ToString(),
                    UrgencyLevel = fr.UrgencyLevel.ToString(),
                    ResolutionDays = (fr.RepairSchedules.Max(rs => rs.UpdatedDate) - fr.ReportDate).TotalDays,
                    RepairCost = fr.RepairSchedules.Sum(rs => rs.RepairCost) ?? 0
                })
                .GroupBy(x => new { x.FaultType, x.UrgencyLevel })
                .Select(g => new
                {
                    g.Key.FaultType,
                    g.Key.UrgencyLevel,
                    AverageResolutionDays = g.Average(x => x.ResolutionDays),
                    AverageRepairCost = g.Average(x => x.RepairCost),
                    CaseCount = g.Count()
                })
                .Where(x => x.CaseCount >= 3) // Only include with sufficient data
                .OrderBy(x => x.AverageResolutionDays)
                .ToListAsync();

            return Json(new { Data = resolutionData, DateRange = new { StartDate = startDate, EndDate = endDate } });
        }

        // ✅ Top Fault-Prone Fridges with Date Range
        [HttpGet]
        public async Task<IActionResult> GetFaultProneFridges(DateTime? startDate, DateTime? endDate)
        {
            // Default to last 30 days if no dates provided
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var faultProneFridges = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate)
                .GroupBy(fr => new { fr.FridgeId, fr.Fridge.Brand, fr.Fridge.Model, fr.Fridge.SerialNumber })
                .Select(g => new
                {
                    FridgeId = g.Key.FridgeId,
                    Brand = g.Key.Brand,
                    Model = g.Key.Model,
                    SerialNumber = g.Key.SerialNumber,
                    TotalFaults = g.Count(),
                    LastFaultDate = g.Max(fr => fr.ReportDate),
                    MostCommonIssue = g.GroupBy(fr => fr.FaultType)
                                      .OrderByDescending(ft => ft.Count())
                                      .Select(ft => ft.Key.ToString())
                                      .FirstOrDefault(),
                    Status = g.OrderByDescending(fr => fr.ReportDate)
                             .Select(fr => fr.Status)
                             .FirstOrDefault()
                })
                .Where(x => x.TotalFaults >= 2) // Fridges with 2 or more faults
                .OrderByDescending(x => x.TotalFaults)
                .Take(15)
                .ToListAsync();

            return Json(new { Data = faultProneFridges, DateRange = new { StartDate = startDate, EndDate = endDate } });
        }

        // ✅ Fault Status Distribution with Date Range
        [HttpGet]
        public async Task<IActionResult> GetFaultStatusDistribution(DateTime? startDate, DateTime? endDate)
        {
            // Default to last 30 days if no dates provided
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var query = _context.FaultReport.Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate);

            var statusData = await query
                .GroupBy(fr => fr.Status)
                .Select(g => new
                {
                    Status = g.Key.ToString(),
                    Count = g.Count(),
                    AverageAgeDays = g.Average(fr => (DateTime.Now - fr.ReportDate).TotalDays)
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return Json(new { Data = statusData, DateRange = new { StartDate = startDate, EndDate = endDate } });
        }

        // GET: Faults/Reports - Main reports page with date range selection
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

        //=================
        //Customer Report side
        //=================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllCustomersSpendingReport()
        {
            var customers = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Fridge)
                .ToListAsync();

            var reportModel = new AdminCustomerSpendingReportViewModel();

            foreach (var cust in customers)
            {
                var orders = (cust.Orders ?? new List<Order>()).ToList();
                var totalSpent = orders.Sum(o => o.TotalAmount);
                var totalFridges = orders.Sum(o => o.OrderItems.Sum(oi => oi.Quantity));

                reportModel.Customers.Add(new CustomerSpendingReportViewModel
                {
                    CustomerName = cust.FullName,
                    Orders = orders,
                    TotalSpent = totalSpent,
                    TotalFridges = totalFridges
                });
            }

            return View(reportModel);
        }
    }
}