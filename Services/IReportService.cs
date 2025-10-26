using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using TaskStatus = FridgeManagementSystem.Models.TaskStatus;
namespace FridgeManagementSystem.Services
{
    public interface IReportService
    {
        Task<DashboardReport> GetDashboardReportAsync();
        Task<DetailedFridgeReport> GetDetailedReportAsync(DateTime? startDate, DateTime? endDate);
        Task<List<FaultReportDetail>> GetFaultReportAsync(string status);
        Task<List<MaintenanceDetail>> GetMaintenanceReportAsync(DateTime? fromDate);
    }

    public class ReportService : IReportService
    {
        private readonly FridgeDbContext _context;

        public ReportService(FridgeDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardReport> GetDashboardReportAsync()
        {
            var report = new DashboardReport();

            // Customer Management Subsystem Data
            report.TotalCustomers = await _context.Customers.CountAsync();
            report.TotalFridges = await _context.Fridge.CountAsync();
            report.AllocatedFridges = await _context.Fridge.CountAsync(f => f.Status == "Allocated");
            report.AvailableFridges = await _context.Fridge.CountAsync(f => f.Status == "Available");

            // Fridge Fault Subsystem Data
            report.PendingFaults = await _context.FaultReport
                .CountAsync(f => f.Status == TaskStatus.Pending);

            report.CompletedFaults = await _context.FaultReport
                .CountAsync(f => f.Status == TaskStatus.Complete);

            // Fridge Maintenance Subsystem Data
            report.ScheduledMaintenance = await _context.MaintenanceVisit
                .CountAsync(m => m.ScheduledDate >= DateTime.Today && m.Status == TaskStatus.Scheduled);

            // Purchasing Subsystem Data
            report.PendingPurchaseRequests = await _context.PurchaseRequests
                .CountAsync(pr => pr.Status == "Pending");

            // Fridge Status Summary
            var fridgeStatuses = await _context.Fridge
                .GroupBy(f => f.Status)
                .Select(g => new FridgeStatusSummary
                {
                    Status = g.Key,
                    Count = g.Count(),
                    Percentage = (decimal)g.Count() / report.TotalFridges * 100
                })
                .ToListAsync();

            report.FridgeStatusSummary = fridgeStatuses;

            // Fault Status Summary
            var faultStatuses = await _context.FaultReport
                .GroupBy(f => f.Status)
                .Select(g => new FaultStatusSummary
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            report.FaultStatusSummary = faultStatuses;

            // Monthly Allocations (last 6 months)
            var sixMonthsAgo = DateTime.Today.AddMonths(-6);
            var monthlyData = await _context.FridgeAllocation
                .Where(a => a.AllocationDate >= sixMonthsAgo)
                .GroupBy(a => new { a.AllocationDate.Year, a.AllocationDate.Month })
                .Select(g => new MonthlyAllocation
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:00}",
                    Allocations = g.Count(),
                    Returns = _context.Fridge.Count(r =>
                        r.ReturnedDate.Year == g.Key.Year && r.ReturnDate.Month == g.Key.Month)
                })
                .ToListAsync();

            report.MonthlyAllocations = monthlyData;

            return report;
        }

        public async Task<DetailedFridgeReport> GetDetailedReportAsync(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Today.AddMonths(-1);
            endDate ??= DateTime.Today;

            var report = new DetailedFridgeReport();

            // Allocations from Customer Management Subsystem
            report.Allocations = await _context.FridgeAllocation
                .Include(a => a.Customer)
                .Include(a => a.Fridge)
                .Where(a => a.AllocationDate >= startDate && a.AllocationDate <= endDate)
                .Select(a => new FridgeAllocationDetail
                {
                    CustomerName = a.Customer.Name,
                    FridgeModel = a.Fridge.Model,
                    SerialNumber = a.Fridge.SerialNumber,
                    AllocationDate = a.AllocationDate,
                    Status = a.Status,
                    Location = a.Customer.Location
                })
                .ToListAsync();

            // Faults from Fridge Fault Subsystem
            report.Faults = await _context.FaultReport
                .Include(f => f.Customer)
                .Include(f => f.Fridge)
                .Include(f => f.Technician)
                .Where(f => f.ReportDate >= startDate && f.ReportDate <= endDate)
                .Select(f => new FaultReportDetail
                {
                    CustomerName = f.Customer.Name,
                    FridgeSerialNumber = f.Fridge.SerialNumber,
                    FaultDescription = f.Description,
                    Status = f.Status,
                    ReportDate = f.ReportDate,
                    ResolutionDate = f.ResolutionDate,
                    Technician = f.Technician != null ? f.Technician.Name : "Unassigned"
                })
                .ToListAsync();

            // Maintenance from Fridge Maintenance Subsystem
            report.Maintenance = await _context.MaintenanceVisit
                .Include(m => m.Customer)
                .Include(m => m.Fridge)
                .Include(m => m.Technician)
                .Where(m => m.MaintenanceDate >= startDate && m.MaintenanceDate <= endDate)
                .Select(m => new MaintenanceDetail
                {
                    CustomerName = m.Customer.Name,
                    FridgeSerialNumber = m.Fridge.SerialNumber,
                    MaintenanceDate = m.MaintenanceDate,
                    MaintenanceType = m.MaintenanceType,
                    Technician = m.Technician.Name,
                    Notes = m.Notes
                })
                .ToListAsync();

            // Purchases from Purchasing Subsystem
            report.Purchases = await _context.PurchaseOrders
                .Include(po => po.Supplier)
                .Where(po => po.OrderDate >= startDate && po.OrderDate <= endDate)
                .Select(po => new PurchaseDetail
                {
                    SupplierName = po.Supplier.Name,
                    FridgeModel = po.Model,
                    Quantity = po.Quantity,
                    UnitPrice = po.UnitPrice,
                    OrderDate = po.OrderDate,
                    Status = po.Status
                })
                .ToListAsync();

            return report;
        }

        public async Task<List<FaultReportDetail>> GetFaultReportAsync(string status)
        {
            var query = _context.FaultReport
                .Include(f => f.Customer)
                .Include(f => f.Fridge)
                .Include(f => f.Technician)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(f => f.Status == status);
            }

            return await query
                .Select(f => new FaultReportDetail
                {
                    CustomerName = f.Customer.Name,
                    FridgeSerialNumber = f.Fridge.SerialNumber,
                    FaultDescription = f.Description,
                    Status = f.Status,
                    ReportDate = f.ReportDate,
                    ResolutionDate = f.ResolutionDate,
                    Technician = f.Technician != null ? f.Technician.Name : "Unassigned"
                })
                .OrderByDescending(f => f.ReportDate)
                .ToListAsync();
        }

        public async Task<List<MaintenanceDetail>> GetMaintenanceReportAsync(DateTime? fromDate)
        {
            fromDate ??= DateTime.Today.AddMonths(-3);

            return await _context.MaintenanceVisit
                .Include(m => m.Customer)
                .Include(m => m.Fridge)
                .Include(m => m.Technician)
                .Where(m => m.MaintenanceDate >= fromDate)
                .Select(m => new MaintenanceDetail
                {
                    CustomerName = m.Customer.Name,
                    FridgeSerialNumber = m.Fridge.SerialNumber,
                    MaintenanceDate = m.MaintenanceDate,
                    MaintenanceType = m.MaintenanceType,
                    Technician = m.Technician.Name,
                    Notes = m.Notes
                })
                .OrderByDescending(m => m.MaintenanceDate)
                .ToListAsync();
        }
    }
}
