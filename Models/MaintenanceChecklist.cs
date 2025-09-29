using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeManagementSystem.Models
{
    public class MaintenanceChecklist
    {
        [Key]
        public int MaintenanceCheckListId { get; set; }

        [EnumDataType(typeof(TemperatureStatus))]
        
        public TemperatureStatus TemperatureStatus { get; set; }


        public bool CondenserCoilsCleaned { get; set; } 

        [EnumDataType(typeof(CoolantLevel))]
        
        public CoolantLevel CoolantLevel { get; set; }

        [EnumDataType(typeof(DoorSealCondition))]
        
        public DoorSealCondition DoorSealCondition { get; set; }

        [EnumDataType(typeof(LightingStatus))]
        
        public LightingStatus LightingStatus { get; set; }

        [EnumDataType(typeof(PowerCableCondition))]
        
        public PowerCableCondition PowerCableCondition { get; set; }

        [StringLength(1000, ErrorMessage = "Additional notes cannot exceed 1000 characters")]
        [BindNever]
        public string? AdditionalNotes { get; set; }
        [ForeignKey("MaintenanceVisit")]
        public int MaintenanceVisitId { get; set; }
        public virtual MaintenanceVisit ? MaintenanceVisit { get; set; }

    }
}
