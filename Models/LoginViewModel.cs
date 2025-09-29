using System.ComponentModel.DataAnnotations;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "Login Role")]
        public string Role { get; set; }
    }
}
