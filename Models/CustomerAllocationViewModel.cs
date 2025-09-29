using System.ComponentModel.DataAnnotations;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class CustomerAllocationViewModel
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public int SelectedFridgeID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly? ReturnDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public string Status { get; set; }

        public List<Fridge> AvailableFridges { get; set; }

        // IDs of fridges selected for allocation (posted back)
        public List<int> SelectedFridgeIDs { get; set; } = new List<int>();

        //how many fridges to allocate
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid quantity")]
        public int QuantityAllocated { get; set; }
    }

}
