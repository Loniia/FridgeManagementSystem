using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.ViewModels;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<Fridge> LowStockFridges { get; set; }
        public IEnumerable<Customer> NewCustomers { get; set; }
        public IEnumerable<FridgeAllocation> PendingAllocations { get; set; }
        public FridgeSummaryViewModel FridgeSummary { get; set; }  
        public Dictionary<string, IEnumerable<Customer>> CustomersByLocation { get; set; } 
        public IEnumerable<PurchaseRequest> PendingPurchaseRequests { get; set; }
        public IEnumerable<MaintenanceRequest> PendingMaintenanceRequests { get; set; }
    }
}
