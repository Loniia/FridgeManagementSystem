namespace FridgeManagementSystem.Models
{
    // ADD THIS CLASS
    public class MaintenanceRequestSummary
    {
        public int MaintenanceRequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string FridgeBrand { get; set; }
        public string FridgeModel { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public int DaysPending { get; set; }
    }

    public class MaintenanceDashboardViewModel
    {
        // YOUR EXISTING PROPERTIES
        public int PendingRequests { get; set; }
        public int ScheduledVisits { get; set; }
        public int RescheduledVisits { get; set; }
        public int InProgressVisits { get; set; }
        public int CompletedTasks { get; set; }
        public int CancelledVisits { get; set; }
        public int CompletionPercent { get; set; }

        public List<string> TrendLabels { get; set; } = new List<string>();
        public List<int> TrendCompletedCounts { get; set; } = new List<int>();
        public List<MaintenanceRequest> TodayVisits { get; set; } = new();
        public List<string> CategoryLabels { get; set; } = new List<string>();
        public List<int> CategoryValues { get; set; } = new List<int>();

        // NEW: Properties for 6-month completed tasks chart
        public List<string> MonthLabels { get; set; } = new List<string>();
        public List<int> CompletedValues { get; set; } = new List<int>();

        // Property for pending requests table
        public List<MaintenanceRequestSummary> PendingRequestsNeedingScheduling { get; set; } = new List<MaintenanceRequestSummary>();
    }
}
