using FridgeManagementSystem.ViewModels;
namespace FridgeManagementSystem.Models
{
    public class ReportDashboardViewModel
    {
        public MaintenanceKpiViewModel MaintenanceKpi { get; set; } = new MaintenanceKpiViewModel();

        // Fridge report data for Chart.js
        public int Received { get; set; }
        public int Purchased { get; set; }
        public int Scrapped { get; set; }
        public int LowStock { get; set; }
        public int Returned { get; set; }

        // Top 10 Customers
        public List<TopCustomerViewModel> TopCustomers { get; set; } = new List<TopCustomerViewModel>();

        // Fault Report Data
        public List<FaultReport> RecentFaults { get; set; } = new List<FaultReport>();
        public int TotalFaultsCount { get; set; }
        public int PendingFaultsCount { get; set; }
        public int HighPriorityFaultsCount { get; set; }
        public int ResolvedFaultsCount { get; set; }
    }
}
