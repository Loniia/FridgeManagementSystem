using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
#nullable disable
namespace FridgeManagementSystem.ViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<Fridge> LowStockFridges { get; set; }
        public IEnumerable<Customer> NewCustomers { get; set; }
        public IEnumerable<PendingAllocationViewModel> PendingAllocations { get; set; }
        public FridgeSummaryViewModel FridgeSummary { get; set; }
        public Dictionary<string, List<Customer>> CustomersByLocation { get; set; }
        public IEnumerable<PurchaseRequest> PendingPurchaseRequests { get; set; }
        public IEnumerable<MaintenanceRequest> PendingMaintenanceRequests { get; set; }
    }
}
