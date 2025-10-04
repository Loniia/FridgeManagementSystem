using System.ComponentModel.DataAnnotations;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class RegisterCustomerViewModel
    {
        [Key]
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [StringLength(10, ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; }
        public int LocationId { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
