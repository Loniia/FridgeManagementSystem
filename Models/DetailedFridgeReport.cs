namespace FridgeManagementSystem.Models
{
    public class DetailedFridgeReport
    {
        public List<FridgeAllocationDetail> Allocations { get; set; }
        public List<FaultReportDetail> Faults { get; set; }
        public List<MaintenanceDetail> Maintenance { get; set; }
        public List<PurchaseDetail> Purchases { get; set; }
    }
}
