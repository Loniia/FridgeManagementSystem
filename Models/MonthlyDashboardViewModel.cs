namespace FridgeManagementSystem.Models
#nullable disable
{
    public class MonthlyDashboardViewModel
    {
        public List<string> Months { get; set; }
        public List<int> ReceivedCounts { get; set; }
        public List<string> ReceivedColors { get; set; }
        public List<int> AllocatedCounts { get; set; }
        public List<int> ReturnedCounts { get; set; }
        public List<int> PurchaseCounts { get; set; }
       
    }
}
