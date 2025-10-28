namespace FridgeManagementSystem.Models
{
    public class MaintenanceKpiViewModel
    {
        // This month's values
        public int CompletedThisMonth { get; set; }
        public double AvgCompletionDays { get; set; }        // average days to complete
        public double SuccessRate { get; set; }              // percent
        public int MonthlyRepeatVisits { get; set; }         // fridges with >1 visit this month
    }
}
