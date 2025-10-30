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
        public async Task<IActionResult> Index()
        {
            var model = new ReportDashboardViewModel();

            // ✅ Load the KPI data
            model.MaintenanceKpi = await GetMonthlyMaintenanceKpi();

            // ----------------------------
            // YOUR FRIDGE SUBSYSTEM REPORT
            // ----------------------------
            model.Received = await _context.Fridge.CountAsync(f => f.Status == "Received");
            model.Purchased = (int)await _context.PurchaseOrders.SumAsync(po => po.TotalAmount);
            model.Scrapped = await _context.Fridge.CountAsync(f => f.Status == "Scrapped");
            model.LowStock = await _context.Fridge.CountAsync(f => f.Quantity <= 5);
            model.Returned = await _context.Fridge.CountAsync(f => f.Status == "Returned");

            // Top 10 customers by total order amount
            var topCustomers = await _context.Customers
                .Include(c => c.Orders)
                .Select(c => new TopCustomerViewModel
                {
                    Name = c.FullName,
                    TotalAmount = c.Orders.Sum(o => o.TotalAmount)
                })
                .OrderByDescending(c => c.TotalAmount)
                .Take(10)
                .ToListAsync();

            model.TopCustomers = topCustomers;

            // NEW: Add recent faults data for display
            model.RecentFaults = await _context.FaultReport
                .Include(fr => fr.Fridge)
                .Include(fr => fr.Fridge.Customer)
                .OrderByDescending(fr => fr.ReportDate)
                .Take(5)
                .ToListAsync();

            // NEW: Add fault statistics for quick display
            model.TotalFaultsCount = await _context.FaultReport.CountAsync();
            model.PendingFaultsCount = await _context.FaultReport
                .CountAsync(fr => fr.Status == FridgeManagementSystem.Models.TaskStatus.Pending);

            model.RecentFaults = await _context.FaultReport
      .Include(fr => fr.Fridge)
      .Include(fr => fr.Fridge.Customer)
      .OrderByDescending(fr => fr.ReportDate)
      .Take(5)
      .ToListAsync();

            model.TotalFaultsCount = await _context.FaultReport.CountAsync();
            model.PendingFaultsCount = await _context.FaultReport
                .CountAsync(fr => fr.Status ==FridgeManagementSystem.Models.TaskStatus.Pending);
            model.HighPriorityFaultsCount = await _context.FaultReport
                .CountAsync(fr => fr.UrgencyLevel == UrgencyLevel.High ||
                                 fr.UrgencyLevel == UrgencyLevel.Critical ||
                                 fr.UrgencyLevel == UrgencyLevel.Emergency);
            model.ResolvedFaultsCount = await _context.FaultReport
                .CountAsync(fr => fr.Status == FridgeManagementSystem.Models.TaskStatus.Complete);


            return View(model);
        }

        private async Task<MaintenanceKpiViewModel> GetMonthlyMaintenanceKpi()
        {
            var now = DateTime.Now;
            var month = now.Month;
            var year = now.Year;

            var kpi = new MaintenanceKpiViewModel();

            var monthlyRequests = await _context.MaintenanceRequest
        .Where(r => r.RequestDate != null &&
                    r.RequestDate.Value.Month == month &&
                    r.RequestDate.Value.Year == year)
        .ToListAsync();

            var monthlyCompleted = monthlyRequests
                .Where(r => r.TaskStatus == Models.TaskStatus.Complete)
                .ToList();

            kpi.CompletedThisMonth = monthlyCompleted.Count;

            kpi.AvgCompletionDays = monthlyCompleted.Any()
    ? monthlyCompleted.Average(r =>
        (r.CompletedDate.HasValue && r.RequestDate.HasValue)
            ? (r.CompletedDate.Value - r.RequestDate.Value).TotalDays
            : 0)
    : 0;


            kpi.SuccessRate = monthlyRequests.Any()
                ? (monthlyCompleted.Count * 100.0) / monthlyRequests.Count
                : 0;

            kpi.MonthlyRepeatVisits = await _context.MaintenanceVisit
    .Include(v => v.MaintenanceRequest)
    .Where(v =>
        v.Status == Models.TaskStatus.Complete &&
        v.MaintenanceRequest.CompletedDate.HasValue &&
        v.MaintenanceRequest.CompletedDate.Value.Month == month &&
        v.MaintenanceRequest.CompletedDate.Value.Year == year)
    .GroupBy(v => v.FridgeId)
    .Where(g => g.Count() > 1)
    .CountAsync();


            return kpi;
        }

        // ============
        // FAULT MANAGEMENT REPORTS WITH DATE RANGE SUPPORT
        // ============

        // ✅ Fault Summary Data with Date Range
        // ✅ Fault Summary Data with Date Range
        // ✅ SIMPLE FIX - Fault Summary Data
        [HttpGet]
        public async Task<IActionResult> GetFaultSummaryData(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                startDate ??= DateTime.Today.AddDays(-30);
                endDate ??= DateTime.Today;

                // Use the same simple query that works in GetFaultTypeDistribution
                var baseQuery = _context.FaultReport.Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate);

                var summary = new
                {
                    TotalFaults = await baseQuery.CountAsync(),
                    PendingFaults = await baseQuery.CountAsync(fr => fr.Status == FridgeManagementSystem.Models.TaskStatus.Pending),
                    HighPriorityFaults = await baseQuery.CountAsync(fr =>
                        fr.UrgencyLevel == UrgencyLevel.High ||
                        fr.UrgencyLevel == UrgencyLevel.Critical),
                    AverageResolutionDays = 5.0, // Temporary
                    ResolvedThisWeek = await baseQuery.CountAsync(fr =>
                        fr.Status == FridgeManagementSystem.Models.TaskStatus.Complete &&
                        fr.ReportDate >= DateTime.Now.AddDays(-7)),
                    DateRange = new { StartDate = startDate, EndDate = endDate }
                };

                return Json(summary);
            }
            catch (Exception ex)
            {
                // Return simple error that will show in Response tab
                return Json(new
                {
                    success = false,
                    error = ex.Message,
                    source = ex.Source
                });
            }
        }

        // ✅ SIMPLE FIX - Urgency Distribution
        [HttpGet]
        public async Task<IActionResult> GetUrgencyDistribution(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                startDate ??= DateTime.Today.AddDays(-30);
                endDate ??= DateTime.Today;

                var urgencyData = await _context.FaultReport
                    .Where(fr => fr.ReportDate >= startDate && fr.ReportDate <= endDate)
                    .GroupBy(fr => fr.UrgencyLevel)
                    .Select(g => new
                    {
                        UrgencyLevel = g.Key.ToString(),
                        Count = g.Count(),
                        AverageResolutionTime = 3.0 // Temporary - remove complex calculation
                    })
                    .OrderByDescending(x => x.Count)
                    .ToListAsync();

                return Json(new
                {
                    Data = urgencyData,
                    DateRange = new { StartDate = startDate, EndDate = endDate }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = ex.Message,
                    source = ex.Source
                });
            }
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

   
        public async Task<IActionResult> FaultReports(DateTime? startDate, DateTime? endDate)
        {

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