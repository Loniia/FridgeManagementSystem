using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FridgeManagementSystem.Migrations;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class FaultReport
    {
        [Key]
        public int FaultReportId { get; set; }

        [Required(ErrorMessage = "Report date is required")]
        [DataType(DataType.Date)]
       
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Fault type is required")]
        [EnumDataType(typeof(FaultType))]
       
        public FaultType FaultType { get; set; }
        [Required(ErrorMessage = "Urgency Level is required")]
        [EnumDataType(typeof(UrgencyLevel))]
        public UrgencyLevel UrgencyLevel { get; set; }

        [Required(ErrorMessage = "Fault description is required")]
        [StringLength(1000, ErrorMessage = "Fault description cannot exceed 1000 characters")]
        public string FaultDescription { get; set; }
        [Required]
        [ForeignKey("Fridge")]
        public int FridgeId { get; set; }
        public string StatusFilter { get; set; }

        public virtual Fridge Fridge { get; set; }
       
        [ForeignKey("MaintenanceVisit")]
        public int MaintenanceVisitId { get; set; }
       
        public virtual MaintenanceVisit? MaintenanceVisit{ get; set; }
        public virtual Fault Fault { get; set; }
        [Required]
        public int FaultID { get; set; }
    }
}
