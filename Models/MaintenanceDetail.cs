namespace FridgeManagementSystem.Models
{
    public class MaintenanceDetail
    {
        public string CustomerName { get; set; }
        public string FridgeSerialNumber { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; }
        public string Technician { get; set; }
        public string Notes { get; set; }
    }
}
