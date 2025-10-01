using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
namespace FridgeManagementSystem.Models
#nullable disable
{
    public class FridgeViewModel
    {
        [ForeignKey("Fridge")]
        public int FridgeId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
        public string CustomerName { get; set; }
        public DateOnly? AllocationDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
    }
}
