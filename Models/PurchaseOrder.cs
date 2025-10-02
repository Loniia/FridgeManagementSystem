using FridgeManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int PurchaseOrderID { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "PO Number")]
        public string PONumber { get; set; } // Auto-generated: "PO-2024-001"

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Which quotation was accepted
        [Required]
        [Display(Name = "Quotation")]
        [ForeignKey("Quotation")]
        public int QuotationID { get; set; }
        public virtual Quotation Quotation { get; set; }

        // Supplier information (via Quotation, but direct link for convenience)
        [Display(Name = "Supplier")]
        public int SupplierID { get; set; }
        public virtual Supplier Supplier { get; set; }

        // Order Details
        [Required]
        [Display(Name = "Total Amount")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Ordered"; // "Ordered", "Confirmed", "Shipped", "Delivered", "Cancelled"

        // Delivery Information
        [DataType(DataType.Date)]
        [Display(Name = "Expected Delivery")]
        public DateTime? ExpectedDeliveryDate { get; set; }

        [StringLength(200)]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }

        [StringLength(500)]
        [Display(Name = "Special Instructions")]
        public string SpecialInstructions { get; set; }

        // Administrative
        public bool IsActive { get; set; } = true;

        // Navigation to Delivery Note (when goods arrive)
        //public virtual DeliveryNote DeliveryNote { get; set; }
    }
}
