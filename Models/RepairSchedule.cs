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

        [Required(ErrorMessage = "Diagnosis is required")]
        [StringLength(500, ErrorMessage = "Diagnosis cannot exceed 500 characters")] // Increased length
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Repair date is required")]
        public DateOnly RepairDate { get; set; }

        [Required(ErrorMessage = "Repair time is required")]
        [DataType(DataType.Time)]
        public TimeSpan ScheduleTime { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; } = "Scheduled"; // Default value

        // Renamed and made optional for initial scheduling
        [StringLength(2000, ErrorMessage = "Repair notes cannot exceed 2000 characters")]
        public string ProgressNotes { get; set; } // Was RepairNotes

        // New properties for repair execution workflow
        public DateTime? StartedAt { get; set; } // When technician started repair
        public DateTime? CompletedAt { get; set; }

        [StringLength(1000, ErrorMessage = "Final notes cannot exceed 1000 characters")]
        public string RepairNotes { get; set; } // Final summary

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RepairCost { get; set; } // Cost of repair

        [StringLength(500, ErrorMessage = "Parts used cannot exceed 500 characters")]
        public string PartsUsed { get; set; }

        // Foreign keys
        [Required(ErrorMessage = "Fault is required")]
        [ForeignKey("Fault")]
        public int FaultID { get; set; }
        public Fault Fault { get; set; }

        [Required(ErrorMessage = "Technician is required")]
        [ForeignKey("FaultTechnician")]
        public int TechnicianID { get; set; }
        public FaultTechnicians FaultTechnician { get; set; }

        [Required(ErrorMessage = "Fridge is required")]
        [ForeignKey("Fridge")]
        public int FridgeID { get; set; }
        public Fridge Fridge { get; set; }

        // Customer association (if needed for notifications)
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public List<RepairSchedule> RecentSchedules { get; set; } = new List<RepairSchedule>();

        // Helper method to check if repair is overdue
        [NotMapped]
        public bool IsOverdue => RepairDate < DateOnly.FromDateTime(DateTime.Today) && Status != "Completed";
        public int ReportID { get; set; }
        public FaultReport FaultReports { get; set; }
    }

}
