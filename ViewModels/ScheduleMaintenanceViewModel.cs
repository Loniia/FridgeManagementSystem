using System.ComponentModel.DataAnnotations;
using FridgeManagementSystem.Models;
#nullable disable
namespace FridgeManagementSystem.ViewModels
{
    public class ScheduleMaintenanceViewModel
    {
        public int FridgeId { get; set; }
        public string FridgeSerialNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ScheduledDate { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public virtual Fridge Fridge { get; set; }
    }
}
