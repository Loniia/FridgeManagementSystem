using System.ComponentModel.DataAnnotations;
#nullable disable
namespace PurchasingSubsystem.Models
{
    public class InventoryLiaison
    {
        [Key]
        public int InventoryLiaisonID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string Email { get; set; }
    }
}
