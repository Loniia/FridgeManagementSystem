using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
#nullable disable
namespace FridgeManagementSystem.ViewModels
{
    public class PaymentViewModel
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }

        public Method Method { get; set; }

        // For Card payments
        public string CardholderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }

        // For EFT payments
        public string BankReference { get; set; }
    }
}
