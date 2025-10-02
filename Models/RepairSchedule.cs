using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class RepairSchedule
    {
        [Key]
        public int RepairID { get; set; }

        [Required]
        [Display(Name = "Fault")]
        public int FaultID { get; set; }

        [Required]
        [Display(Name = "Fridge")]
        public int FridgeId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Diagnosing"; // Diagnosing, Awaiting Parts, Repairing, Testing, Completed

        [Display(Name = "Diagnosis")]
        [StringLength(1000)]
        public string Diagnosis { get; set; }

        [Display(Name = "Repair Type")]
        [StringLength(100)]
        public string RepairType { get; set; } // Electrical, Mechanical, Refrigeration, etc.

        [Display(Name = "Repair Notes")]
        [StringLength(2000)]
        public string RepairNotes { get; set; }

        [Display(Name = "Parts Used")]
        [StringLength(500)]
        public string PartsUsed { get; set; }

        [Display(Name = "Required Parts")]
        [StringLength(500)]
        public string RequiredParts { get; set; }

        [Display(Name = "Repair Cost")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 10000, ErrorMessage = "Repair cost must be between 0 and 10,000")]
        public decimal? RepairCost { get; set; }

        [Display(Name = "Actual Repair Time (hours)")]
        [Range(0.1, 24, ErrorMessage = "Repair time must be between 0.1 and 24 hours")]
        public decimal? ActualRepairTime { get; set; }

        // Dates for tracking progress
        [Display(Name = "Diagnosis Date")]
        public DateTime? DiagnosisDate { get; set; }

        [Display(Name = "Repair Start Date")]
        public DateTime? RepairStartDate { get; set; }

        [Display(Name = "Repair End Date")]
        public DateTime? RepairEndDate { get; set; }

        [Display(Name = "Testing Date")]
        public DateTime? TestingDate { get; set; }

        [Display(Name = "Completion Date")]
        public DateTime? CompletionDate { get; set; }
        [Display(Name = "Repair Date")]
        public DateTime? RepairDate { get; set; }

        // Technician information
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Display(Name = "Technician")]
        public int? TechnicianID { get; set; }

        // Audit fields
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("FaultID")]
        public virtual Fault Fault { get; set; }

        [ForeignKey("FridgeId")]
        public virtual Fridge Fridge { get; set; }

        [ForeignKey("Employee")]
        public virtual Employee FaultTechnician { get; set; }

        // Helper methods
        [NotMapped]
        public bool IsCompleted => Status == "Completed";

        [NotMapped]
        public bool NeedsParts => !string.IsNullOrEmpty(RequiredParts) && Status == "Awaiting Parts";

        [NotMapped]
        public string CurrentStage
        {
            get
            {
                return Status switch
                {
                    "Diagnosing" => "Diagnosis Phase",
                    "Awaiting Parts" => "Waiting for Parts",
                    "Repairing" => "Repair Phase",
                    "Testing" => "Testing Phase",
                    "Completed" => "Completed",
                    _ => "Unknown"
                };
            }
        }

        // Method to calculate total repair duration
        [NotMapped]
        public TimeSpan? RepairDuration
        {
            get
            {
                if (RepairStartDate.HasValue && RepairEndDate.HasValue)
                    return RepairEndDate.Value - RepairStartDate.Value;
                return null;
            }
        }
    }
}

