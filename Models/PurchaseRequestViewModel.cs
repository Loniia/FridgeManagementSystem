namespace FridgeManagementSystem.Models
#nullable disable
{
    public class PurchaseRequestViewModel
    {
        public int PurchaseRequestID { get; set; }
        public DateOnly RequestDate { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public int Quantity { get; set; }
        public string ItemName { get; set; }
    }
}
