using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class Fault
    {
        [Key]
        public int FaultID { get; set; }

        [Required(ErrorMessage = "Fault Description is required")]
        [StringLength(500, ErrorMessage = "Fault Description cannot exceed 500 characters")]
        public string FaultDescription { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending"; // Pending, Diagnosing, Repairing, Testing, Resolved

        [Required]
        [Display(Name = "Priority")]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High

        public DateTime ScheduledDate { get; set; }

        [Display(Name = "Date Reported")]
        [DataType(DataType.Date)]
        public DateTime ReportDate { get; set; } = DateTime.Now;
        [Required ]
        public string Notes {  get; set; }
        [Required(ErrorMessage = "Appliance Type is required")]
        [StringLength(100, ErrorMessage = "Appliance Type cannot exceed 100 characters")]
        [Display(Name = "Appliance Type")]
        public string ApplianceType { get; set; } = string.Empty;

        // Additional fields for fault processing
        [Display(Name = "Initial Assessment")]
        [StringLength(1000)]
        public string InitialAssessment { get; set; }

        [Display(Name = "Estimated Repair Time (hours)")]
        [Range(0.5, 24, ErrorMessage = "Repair time must be between 0.5 and 24 hours")]
        public decimal? EstimatedRepairTime { get; set; }

        [Display(Name = "Required Parts")]
        [StringLength(500)]
        public string RequiredParts { get; set; }

        [Display(Name = "Is Urgent?")]
        public bool IsUrgent { get; set; } = false;
        
        // Audit fields
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // Technician who is processing the fault
        [Display(Name = "Technician")]
        public int? TechnicianID { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fridge is required")]
        public int FridgeId { get; set; }
        public virtual Fridge Fridge { get; set; }

        // Navigation property for related repair schedules
        public virtual ICollection<RepairSchedule> RepairSchedules { get; set; }
    }
}
 
 