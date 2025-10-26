using FridgeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FridgeManagementSystem.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var dashboardReport = await _reportService.GetDashboardReportAsync();
            return View(dashboardReport);
        }

        public async Task<IActionResult> DetailedReport(DateTime? startDate, DateTime? endDate)
        {
            var detailedReport = await _reportService.GetDetailedReportAsync(startDate, endDate);

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd") ?? DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd") ?? DateTime.Today.ToString("yyyy-MM-dd");

            return View(detailedReport);
        }

        public async Task<IActionResult> FaultReport(string status)
        {
            var faultReport = await _reportService.GetFaultReportAsync(status);
            ViewBag.SelectedStatus = status ?? "All";
            return View(faultReport);
        }

        public async Task<IActionResult> MaintenanceReport(DateTime? fromDate)
        {
            var maintenanceReport = await _reportService.GetMaintenanceReportAsync(fromDate);
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd") ?? DateTime.Today.AddMonths(-3).ToString("yyyy-MM-dd");
            return View(maintenanceReport);
        }

        public IActionResult ExportToExcel(string reportType, DateTime? startDate, DateTime? endDate, string status)
        {
            // Implementation for Excel export
            // You can use libraries like EPPlus or ClosedXML
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> GetDashboardData()
        {
            var dashboardData = await _reportService.GetDashboardReportAsync();
            return Json(dashboardData);
        }
    }
}
