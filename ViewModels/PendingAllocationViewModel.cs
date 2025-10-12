#nullable disable
namespace FridgeManagementSystem.ViewModels
{
    public class PendingAllocationViewModel
    {
        public int OrderItemId { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int FridgeId { get; set; }
        public string FridgeBrand { get; set; }
        public string FridgeModel { get; set; }
        public int QuantityOrdered { get; set; }
        public int QuantityAllocated { get; set; } // Already allocated
        public int QuantityPending => QuantityOrdered - QuantityAllocated; // Auto-computed
        public string Status { get; set; }
    }
}
