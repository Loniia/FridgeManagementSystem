namespace FridgeManagementSystem.Models
{
    public class DashboardReport
    {
        public int TotalCustomers { get; set; }
        public int TotalFridges { get; set; }
        public int AllocatedFridges { get; set; }
        public int AvailableFridges { get; set; }
        public int PendingFaults { get; set; }
        public int CompletedFaults { get; set; }
        public int ScheduledMaintenance { get; set; }
        public int PendingPurchaseRequests { get; set; }
        //public List<FridgeStatusSummary> FridgeStatusSummary { get; set; }
        //public List<FaultStatusSummary> FaultStatusSummary { get; set; }
        //public List<MonthlyAllocation> MonthlyAllocations { get; set; }
    }
}
