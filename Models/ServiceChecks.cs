using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeManagementSystem.Models
#nullable disable
{
    public class ServiceChecks
    {
        [Key]
        public int ServiceCheckId { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        [StringLength(200)]
        public string CustomerName {  get; set; }
        [Required]
        [StringLength(200)]
        public string Model {  get; set; }
        public DateOnly LastServiced {  get; set; }= DateOnly.FromDateTime(DateTime.Now);
        [ForeignKey("Location")]
        public int Location {  get; set; }
        [Required]
        [StringLength(200)]
        public string ServiceDetails {  get; set; }
        [Required]
        [StringLength(200)]
        public string Result {  get; set; }
        [ForeignKey("Fridge")]
        public int FridgeId { get; set; }
        [ForeignKey("Technician")]
        public int TechnicianID { get; set; }
    }
}
