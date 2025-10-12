using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;

#nullable disable
namespace FridgeManagementSystem.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        public string ContactName { get; set; }

        [Required]
        public string ContactPhone { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal TotalAmount { get; set; }
       
        public Method PaymentMethod { get; set; }

        // Optional details depending on payment type
        public string CardNumber { get; set; }
        public string BankReference { get; set; }
    }
}
