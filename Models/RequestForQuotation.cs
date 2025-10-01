using FridgeManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class RequestForQuotation
    {
        [Key]
        public int RFQID { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "RFQ Number")]
        public string RFQNumber { get; set; } // "RFQ-2024-001"

        [Required]
        [Display(Name = "Purchase Request")]
        [ForeignKey("PurchaseRequest")]
        public int PurchaseRequestID { get; set; }
        public virtual PurchaseRequest PurchaseRequest { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; } = DateTime.Now.AddDays(7);

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Draft"; // "Draft", "Sent", "Closed"

        

        // Navigation Properties 
        public virtual ICollection<Quotation> Quotations { get; set; }
    }
}
