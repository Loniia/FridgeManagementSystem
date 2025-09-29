namespace CustomerManagementSubSystem.Models
{
    public class MonthlyFridgeStatsViewModel
    {
        public List<string> Months { get; set; } = new List<string>(); 
        public List<int> ReceivedCounts { get; set; } = new List<int>();
        public List<int> AllocatedCounts { get; set; } = new List<int>();
        public List<int> ReturnedCounts { get; set; } = new List<int>();
    }
}
