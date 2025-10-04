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
        public string CardNumber { get; set; } // Optional if paying by card
        public string BankReference { get; set; } // Optional if paying by EFT
    }
}
