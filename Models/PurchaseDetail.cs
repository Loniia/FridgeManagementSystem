namespace FridgeManagementSystem.Models
{
    public class PurchaseDetail
    {
        public string SupplierName { get; set; }
        public string FridgeModel { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}
