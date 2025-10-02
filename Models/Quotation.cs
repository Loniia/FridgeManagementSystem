using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class Quotation
    {
        [Key]
        public int QuotationID { get; set; }

        [Required(ErrorMessage = "RFQ is required.")]
        [Display(Name = "Request for Quotation")]
        public int RequestForQuotationId { get; set; }

        [Required(ErrorMessage = "Supplier is required.")]
        public int SupplierId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Received Date")]
        public DateTime ReceivedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Quotation Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quotation Amount must be greater than 0.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Quotation Amount")]
        public decimal QuotationAmount { get; set; }

        [StringLength(20)]
        [Display(Name ="Status")]
        public string Status { get; set; }
        // Navigation Properties
        public virtual RequestForQuotation RequestForQuotation { get; set; }

        public virtual Supplier Supplier { get; set; }

        //navigation property
 

    }
}
