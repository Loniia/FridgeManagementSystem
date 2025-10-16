using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class FridgeAllocation
    {
        [Key]
        public int AllocationID { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [Required]
        [ForeignKey("Fridge")]
        public int FridgeId { get; set; }
        [Required]
        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }
       
        [Required]
        [DataType(DataType.Date)]
        public DateOnly AllocationDate { get; set; }= DateOnly.FromDateTime(DateTime.Now);

        [DataType(DataType.Date)]
        public DateOnly? ReturnDate { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Status cannot be longer than 50 characters")]
        public string Status { get; set; } 
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int QuantityAllocated { get; set; }
       
        //Navigation Property
        public virtual Customer Customer { get; set; }
        public virtual Fridge Fridge { get; set; }
        public virtual OrderItem OrderItem { get; set; }
      
    }
}
