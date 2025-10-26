namespace FridgeManagementSystem.Models
{
    public class FridgeAllocationDetail
    {
        public string CustomerName { get; set; }
        public string FridgeModel { get; set; }
        public string SerialNumber { get; set; }
        public DateTime AllocationDate { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
    }
}
