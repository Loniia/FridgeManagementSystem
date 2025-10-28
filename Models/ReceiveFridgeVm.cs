using System.ComponentModel.DataAnnotations;
#nullable disable

namespace CustomerManagementSubSystem.Models
{
    public class ReceiveFridgeVm
    {
        [Required]
        public int FridgeId { get; set; }   // 🆕 Link to the fridge

      
        [StringLength(50)]
        public string Brand { get; set; }

        [Required]
        [StringLength(50)]
        public string ModelName { get; set; }

      
        public string Type { get; set; }

       
        [StringLength(100)]
        public string SerialNumber { get; set; }

        [Required]
        public int SupplierId { get; set; } // dropdown to select supplier

        [Required]
        public int Quantity { get; set; }  // 🆕 Number of fridges received

        [Required]
        public DateOnly? DateAdded { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        //  Status field for Received/Available
        [Required(ErrorMessage = "Please select a status")]
        [StringLength(20)]
        public string Status { get; set; } = "Received"; // Default to Received

        //  To track which purchase request this came from
        public int? PurchaseRequestID { get; set; }

    }
}
