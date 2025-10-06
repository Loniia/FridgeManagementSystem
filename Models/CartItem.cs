using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }
    }
}
