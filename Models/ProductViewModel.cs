using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public int AvailableStock { get; set; }
        public string ImageUrl => Product?.ImageUrl;  // Or Product.Fridge.ImageUrl if linked
        public decimal Price => Product?.Price ?? 0;
    }
}
