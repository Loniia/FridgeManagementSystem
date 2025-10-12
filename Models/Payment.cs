using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Orders { get; set; }

        [Required(ErrorMessage = "Payment amount is required")]
        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [StringLength(50)]
        [EnumDataType(typeof(Method))]
        public Method Method { get; set; }  // enum, not string
        //public string CardholderName { get; set; } // For Card payments
        public string BankReference { get; set; } // For EFT payments
        public string CardNumber { get; set; } // For Card payments
                                               // For EFT
        public string PaymentReference { get; set; } // new unique reference for EFT
        public string ProofFilePath { get; set; } // for EFT proof upload
        public string Status { get; set; } // "Pending", "AwaitingAdminApproval", "Paid"
        public DateTime PaymentDate { get; set; }
        

       

    }
}
