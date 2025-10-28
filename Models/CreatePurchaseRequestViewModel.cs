using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class CreatePurchaseRequestViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int? FridgeId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? InventoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string ItemFullNames { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public int CustomerID { get; set; }
        public string RequestBy { get; set; } = "Inventory Liaison";
        public string RequestType { get; set; } = "Fridge Purchase";
        [Required(ErrorMessage = "Please select a request date")]
        [DataType(DataType.Date)]
        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; }
    }
}
