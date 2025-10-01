using FridgeManagementSystem.Models;

namespace FridgeManagementSystem.Models
#nullable disable
{
    public class FridgeAllocationViewModel
    {
        public int AllocationID { get; set; }
        public int FridgeId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public int QuantityAllocated { get; set; }
        public DateOnly? AllocationDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public FridgeViewModel fridge { get; set; }

    }
}
