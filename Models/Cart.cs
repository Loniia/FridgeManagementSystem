using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual Customer Customer { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
