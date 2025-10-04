using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
#nullable disable

namespace FridgeManagementSystem.ViewModels
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
