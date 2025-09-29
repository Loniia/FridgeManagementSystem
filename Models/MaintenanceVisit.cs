using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeManagementSystem.Models
{
    public class MaintenanceVisit
    {
        [Key]
        public int MaintenanceVisitId { get; set; }
        [ForeignKey("Fridge")]
        [Required(ErrorMessage = "Please select a fridge.")]
        public int FridgeId { get; set; }
        public Fridge? Fridge { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        public Employee? Employee { get; set; }
        [ForeignKey("MaintenanceRequest")]
        public int MaintenanceRequestId { get; set; }
        public MaintenanceRequest? MaintenanceRequest { get; set; }

        [Required(ErrorMessage = "Please select a date.")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Please select a time.")]
        public TimeSpan ScheduledTime { get; set; }

        [StringLength(1000)]
        public string? VisitNotes { get; set; }

        public virtual ICollection<FaultReport>? FaultReport { get; set; }
        public virtual ICollection<ComponentUsed>? ComponentUsed { get; set; }
        public virtual MaintenanceChecklist? MaintenanceChecklist { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Pending;
    }
}



               