using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class Employee
    {

        [Key]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ErrorMessage = "Contact info must be a valid email")]
        public string ContactInfo { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } // Example: "Technician", "Customer Liaison", etc.

        [Required]
        [RegularExpression("^(Active|Inactive)$",
            ErrorMessage = "Status must be 'Active' or 'Inactive'")]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        [ForeignKey("ApplicationUserId")]
        [Required]
        public int ApplicationUserId { get; set; }
        public ApplicationUser UserAccount { get; set; }

        public virtual ICollection<Fault> Faults { get; set; } = new List<Fault>();
        public virtual ICollection<RepairSchedule> RepairSchedules { get; set; } = new List<RepairSchedule>();

        [NotMapped]
        public int CompletedRepairsCount => RepairSchedules?.Count(r => r.Status == "Completed") ?? 0;
    }
}
