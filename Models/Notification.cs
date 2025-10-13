using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeManagementSystem.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        // The user id this notification is for (Admin or Customer). Use your user / customer id type.
        public int UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        public string Url { get; set; } // optional link the notification points to

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
