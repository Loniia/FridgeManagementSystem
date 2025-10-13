namespace FridgeManagementSystem.ViewModels
{
    public class CustomerOrderItemViewModel
    {
        public int OrderItemId { get; set; }
        public int FridgeId { get; set; }
        public string FridgeBrand { get; set; } = string.Empty;
        public string FridgeModel { get; set; } = string.Empty;
        public int OrderedQuantity { get; set; }
        public int AlreadyAllocated { get; set; }
        public int RemainingToAllocate { get; set; }
        public decimal UnitPrice { get; set; }
        public int FridgeStock { get; set; }
        public string FridgeName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int QuantityToAllocate { get; set; } = 1;
        public DateOnly ReturnDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddMonths(12)); // Default 1 year
    }
}
