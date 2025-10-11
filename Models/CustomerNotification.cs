using FridgeManagementSystem.Data;
using FridgeManagementSystem.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class CustomerNotification
    {
        [Key]
        public int CustomerNotificationId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
    }
}
