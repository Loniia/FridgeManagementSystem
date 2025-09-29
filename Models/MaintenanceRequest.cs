using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeManagementSystem.Models
{
    public class MaintenanceRequest
    {
        [Key]
        public int MaintenanceRequestId { get; set; }

        [DataType(DataType.Date)]

        public DateTime RequestDate { get; set; } = DateTime.Now;

        [EnumDataType(typeof(TaskStatus))]
        public TaskStatus TaskStatus { get; set; } = TaskStatus.Pending;
        public bool IsActive { get; set; } = true;
        [ForeignKey("Fridge")]
        public int FridgeId { get; set; }
        public virtual Fridge Fridge { get; set; }



        public virtual MaintenanceVisit MaintenanceVisit { get; set; }
    }
}
