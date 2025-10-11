using System.ComponentModel.DataAnnotations;
namespace FridgeManagementSystem.Models
{
    public class CreatePurchaseRequestViewModel
    {
            [Required]
            [StringLength(100)]
            public string ItemFullNames { get; set; }

            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
            public int Quantity { get; set; }

            [Required]
            public int CustomerID { get; set; }
        
    }
}
