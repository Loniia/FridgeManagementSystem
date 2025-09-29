using System.ComponentModel.DataAnnotations;
#nullable disable
namespace PurchasingSubsystem.Models
{
    public class PurchaseManager
    {
        [Key]
        public int PurchaseManagerID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string PurchaseManagerName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string PurchaseManagerEmail { get; set; }
    }
}
