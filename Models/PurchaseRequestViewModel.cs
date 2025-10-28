using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FridgeManagementSystem.Models
#nullable disable
{
    public class PurchaseRequestViewModel
    {
        [HiddenInput(DisplayValue = false)] // 👈🏽 hides it from UI
        public int PurchaseRequestID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? FridgeId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? InventoryID { get; set; }

        public DateOnly? RequestDate { get; set; }

        [Required]
        [StringLength(100)]
        public string ItemFullNames { get; set; }

        [Required]
        [StringLength(100)]
        public string RequestBy { get; set; }

        public string AssignedToRole { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        [StringLength(50)]
        public string RequestType { get; set; }

        [Required]
        [StringLength(20)]
        public string RequestNumber { get; set; }

        [Required]
        public int Quantity { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsViewed { get; set; }
        public DateTime? ViewedDate { get; set; }

        // Nice display names for UI
        [Display(Name = "Fridge Name")]
        public string FridgeName { get; set; }

        [Display(Name = "Inventory Item")]
        public string InventoryName { get; set; }
        public Fridge Fridge { get; set; }
        [NotMapped]
        public string DisplayStatus
        {
            get
            {
                if (Status == "Approved" && Fridge != null && Fridge.Status != "Available")
                    return "Approved — Receive Fridge";
                return Status;
            }
        }


    }
}
