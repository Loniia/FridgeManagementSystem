using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace PurchasingSubsystem.Models
{
    public class DeliveryNote
    {
        [Key]
        public int DeliveryNoteID { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Delivery Note Number")]
        public string DeliveryNumber { get; set; } 

        [Required]
        [Display(Name = "Purchase Order")]
        [ForeignKey("PurchaseOrder")]
        public int PurchaseOrderID { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; } = DateTime.Today;

        // What was actually delivered
        [Required]
        [Display(Name = "Quantity Delivered")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int QuantityDelivered { get; set; }

        // Delivery verification
        [Display(Name = "Received By")]
        [StringLength(100)]
        public string ReceivedBy { get; set; } // Name of person who accepted delivery

        [Display(Name = "Delivery Verified")]
        public bool IsVerified { get; set; } = false;

        [DataType(DataType.DateTime)]
        [Display(Name = "Verification Date")]
        public DateTime? VerificationDate { get; set; }

        // Connection to Inventory Subsystem
        [Display(Name = "Received in Inventory")]
        public bool IsReceivedInInventory { get; set; } = false;

        [DataType(DataType.DateTime)]
        [Display(Name = "Inventory Receipt Date")]
        public DateTime? InventoryReceiptDate { get; set; }

        // Notes
        [StringLength(500)]
        [Display(Name = "Delivery Notes")]
        public string Notes { get; set; }

        // Administrative
        public bool IsActive { get; set; } = true;
    }

    
}

