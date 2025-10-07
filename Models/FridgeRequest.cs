using System.ComponentModel.DataAnnotations;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class FridgeRequest
    {
        [Key]
        public int FridgeRequestID {  get; set; }
        public int RequestId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string RequiredModel { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        public DateTime RequiredDate { get; set; }

        [StringLength(500)]
        public string SpecialRequirements { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Fulfilled

        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }

        [StringLength(50)]
        public string RequestCode { get; set; }

        [StringLength(500)]
        public string AdminNotes { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
    }
}
