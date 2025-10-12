using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Fridge")]
        public int FridgeId { get; set; }
        public virtual Fridge Fridge { get; set; }
        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
        public string AllocationStatus { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }

       
    }
}
