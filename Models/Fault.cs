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
        public string Status { get; set; } = "Pending"; // Default value

        public string Priority { get; set; } = "Low";

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

        [Required(ErrorMessage = "Customer is required")]
        [Display(Name = "Customer ID")]
        public string CustomerID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer Name is required")]
        [StringLength(200, ErrorMessage = "Customer Name cannot exceed 200 characters")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

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
 
 