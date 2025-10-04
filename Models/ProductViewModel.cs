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
    }
}
