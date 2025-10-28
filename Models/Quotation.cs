using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class Quotation
    {
        [Key]
        public int QuotationID { get; set; }

        [Required(ErrorMessage = "RFQ is required.")]
        [Display(Name = "Request for Quotation")]
        [ForeignKey("RequestForQuotation")]
        public int RequestForQuotationId { get; set; }
        public virtual RequestForQuotation RequestForQuotation { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        [ForeignKey("Supplier")]
        public int?SupplierID { get; set; }
        public virtual Supplier Supplier { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Received Date")]
        public DateTime ReceivedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Quotation Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quotation Amount must be greater than 0.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Quotation Amount")]
        public decimal QuotationAmount { get; set; }

        [Display(Name = "Description")]
        [StringLength(300)]
        public string Description { get; set; }

        [Display(Name = "Required Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public decimal RequiredQuantity { get; set; }

        [StringLength(20)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Submitted"; // Submitted, Approved, Rejected

        // ❌ REMOVED THESE PROPERTIES - THEY DON'T EXIST IN DATABASE:
        // public int? DeliveryTimeframe { get; set; }
        // public DateTime? ValidUntil { get; set; } 
        // public string TermsAndConditions { get; set; }
    }
}