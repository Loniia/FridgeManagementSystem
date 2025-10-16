using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>(); // ✅ Initialize list to prevent null errors
            Payments = new List<Payment>();     // ✅ Also initialize Payments for safety
        }

        [Key]
        public int OrderId { get; set; }

        [ForeignKey("Customers")]
        public int CustomerID { get; set; }
        public virtual Customer Customers { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        [Required]
        [Range(0.01, 1000000)]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Order status is required")]
        [StringLength(50)]
        public string Status { get; set; } // Processing, Packed, etc.

        [Required(ErrorMessage = "Order status is required")]
        [StringLength(50)]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus OrderProgress { get; set; }// use enum

        [Required(ErrorMessage = "Delivery address is required")]
        [StringLength(500)]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Contact Name is required")]
        [StringLength(50)]
        public string ContactName { get; set; }
        [Required(ErrorMessage = "Contact Phone is required")]
        [StringLength(50)]
        public string ContactPhone { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
