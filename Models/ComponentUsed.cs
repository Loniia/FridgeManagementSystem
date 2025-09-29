using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeManagementSystem.Models
{
    public class ComponentUsed
    {
        [Key]
        public int ComponentUsedId { get; set; }

        [Required(ErrorMessage = "Component name is required")]
        [StringLength(100, ErrorMessage = "Component name cannot exceed 100 characters")]
        public string ComponentName { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Condition is required")]
        [EnumDataType(typeof(ComponentCondition))]
        public ComponentCondition Condition { get; set; }

        [ForeignKey("MaintenanceVisit")]
        public int MaintenanceVisitId { get; set; }
        public virtual MaintenanceVisit MaintenanceVisit { get; set; }
    }
}
