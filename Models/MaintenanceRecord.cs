namespace FridgeManagementSystem.Models
{
    public class MaintenanceRecord
    {
        public int MaintenanceRecordId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Description { get; set; }

        public int FridgeId { get; set; }
        public Fridge Fridge { get; set; }
    }
}
