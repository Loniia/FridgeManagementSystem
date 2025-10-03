using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.ViewModels;

#nullable disable
namespace FridgeManagementSystem.Models
{
    public class CheckoutViewModel
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public string DeliveryAddress { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
