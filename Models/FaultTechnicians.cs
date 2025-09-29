using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class FaultTechnicians
    {
        [Key]
        public int TechnicianID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string Specialization { get; set; } // Electrical, Mechanical, Refrigeration

        public bool IsActive { get; set; } = true;

        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; } = DateTime.Today;

        // Navigation properties
        public virtual ICollection<Fault> Faults { get; set; } = new List<Fault>();
        public virtual ICollection<RepairSchedule> RepairSchedules { get; set; } = new List<RepairSchedule>();

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public int CompletedRepairsCount => RepairSchedules?.Count(r => r.Status == "Completed") ?? 0;
    }
}
