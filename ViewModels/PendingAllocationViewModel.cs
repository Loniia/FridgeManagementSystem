using FridgeManagementSystem.Models;
#nullable disable
namespace FridgeManagementSystem.ViewModels
{
    public class PendingAllocationViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public int OrderItemId { get; set; }
        public string FridgeBrand { get; set; }
        public string FridgeModel { get; set; }
        public int QuantityAllocated { get; set; }
        public int QuantityOrdered { get; set; }
        public int QuantityPending { get; set; }
        public string Status { get; set; }
        public int FridgeId { get; set; }
    }
}
