using System.ComponentModel.DataAnnotations;

namespace FridgeManagementSystem.Models
#nullable disable
{
    public class RegisterEmployeeViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
        public string Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string EmployeeRole { get; set; }

    }
}
