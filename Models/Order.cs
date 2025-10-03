using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Order status is required")]
        [StringLength(50)]
        public string Status { get; set; } // Processing, Packed, etc.

        [Required]
        [Range(0.01, 1000000)]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Delivery address is required")]
        [StringLength(500)]
        public string DeliveryAddress { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }
    }
}
