using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace FridgeManagementSystem.ViewModels
{
    public class PaymentViewModel
    {
        [Key]
        public int OrderId { get; set; }
        public decimal Amount { get; set; }

        [Required]
        [EnumDataType(typeof(Method))]
        public Method Method { get; set; }

        // For Card payments
        public string CardholderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public Order Orders { get; set; }
        // For EFT payments
        public string BankReference { get; set; }
        public IFormFile ProofOfPayment { get; set; }
    }
}
