using System.ComponentModel.DataAnnotations;
using FridgeManagementSystem.ViewModels;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class CustomerAllocationViewModel
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a fridge")]
        public int SelectedFridgeID { get; set; }

        [Required(ErrorMessage = "Return date is required")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Return date cannot be in the past")]
        public DateOnly? ReturnDate { get; set; } 

        public string Status { get; set; } = "Pending";

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int QuantityAllocated { get; set; } = 1;

        // Fridges available to allocate
        public List<Fridge> AvailableFridges { get; set; } = new List<Fridge>();

        // Customer's order items
        public List<CustomerOrderItemViewModel> OrderItems { get; set; } = new List<CustomerOrderItemViewModel>();
    }

    // Optional: Add custom validation attribute for future dates
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date >= DateTime.Today;
            }
            return false;
        }
    }
}
