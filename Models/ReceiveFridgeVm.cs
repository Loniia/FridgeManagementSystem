using System.ComponentModel.DataAnnotations;
#nullable disable

namespace CustomerManagementSubSystem.Models
{
    public class ReceiveFridgeVm
    {
        [Required]
        public int FridgeId { get; set; }   // 🆕 Link to the fridge

        [Required]
        [StringLength(50)]
        public string Brand { get; set; }

        [Required]
        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [StringLength(100)]
        public string SerialNumber { get; set; }

        [Required]
        public int SupplierId { get; set; } // dropdown to select supplier

        [Required]
        public int Quantity { get; set; }  // 🆕 Number of fridges received

        [Required]
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
