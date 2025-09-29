using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class FaultTechnicians
    {
        [Key]
        public int TechnicianID { get; set; }

        [Required(ErrorMessage = "Technician Name is required")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber {  get; set; }
        public string Specialization {  get; set; } 
        public string EmployeeNumber {  get; set; }
        [ForeignKey("RepairSchedule")]
        public int RepairID { get; set; }

        public ICollection<FaultReport> AssignedFaults { get; set; } = new List<FaultReport>();
    }
}
