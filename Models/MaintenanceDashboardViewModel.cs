namespace FridgeManagementSystem.Models
{
    public class MaintenanceDashboardViewModel
    {
        // summary counts
        public int PendingRequests { get; set; }
        public int ScheduledVisits { get; set; }
        public int RescheduledVisits { get; set; }
        public int InProgressVisits { get; set; }
        public int CompletedTasks { get; set; }
        public int CancelledVisits { get; set; }

        // percent complete (0-100)
        public int CompletionPercent { get; set; }

        // trend: last N days labels & values for completed tasks
        public List<string> TrendLabels { get; set; } = new List<string>();
        public List<int> TrendCompletedCounts { get; set; } = new List<int>();
        public List<MaintenanceRequest> TodayVisits { get; set; } = new();
        // bar chart categories counts (same as above but easier for JS)
        public List<string> CategoryLabels { get; set; } = new List<string>();
        public List<int> CategoryValues { get; set; } = new List<int>();
    }
}
