using System.ComponentModel.DataAnnotations;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class Administrator
    {
        [Key]
        public int AdminID { get; set; } 
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string PasswordHash { get; set; } // Store hashed password

        [Required]
        [RegularExpression("^(SuperAdmin|Manager|Clerk)$",
            ErrorMessage = "Role must be either 'SuperAdmin'")]
        public string Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        //[Key]
        //public int AdminID { get; set; }

        //[Required]
        //public string Name { get; set; }

        //[Required, EmailAddress]
        //public string Email { get; set; }

        //public string Role { get; set; } // Optional: SuperAdmin, etc.

        //// Navigation
        //public ICollection<Employee> Employees { get; set; }
    }
}
