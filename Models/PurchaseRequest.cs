using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class PurchaseRequest
    {
        [Key]
        public int PurchaseRequestID { get; set; }

        [ForeignKey("Fridge")]
        public int FridgeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly RequestDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
       
        [Required]
        [StringLength(100)]
        public string ItemFullNames { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Requested by name cannot exceed 100 characters")]
        public string RequestBy { get; set; }

        public string AssignedToRole { get; set; } // e.g., "PurchasingManager" for the Create Purchase request form
        [Required]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; }// e.g., "Pending", "Approved", "Rejected"

        [Required]
        [StringLength(50, ErrorMessage = "Request type cannot exceed 50 characters")]
        public string RequestType { get; set; }
        [Required]
        [StringLength(20)]
        public string RequestNumber { get; set; }
        [Required]
        public int Quantity { get; set; }
        public bool IsActive { get; set; } = true;
        public int CustomerID { get; set; }

        // fields for tracking the Purchase from Purchasing Manager
        public bool IsViewed { get; set; } // True if manager has opened it
        public DateTime? ViewedDate { get; set; } // When it was viewed

        //Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual Fridge Fridge { get; set; }

        [ForeignKey("InventoryLiaisonId")]
        public int InventoryLiaisonID { get; set; }

    }
}
