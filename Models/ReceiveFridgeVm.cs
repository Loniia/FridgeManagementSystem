using System.ComponentModel.DataAnnotations;
#nullable disable

namespace CustomerManagementSubSystem.Models
{
    public class ReceiveFridgeVm
    {
        //How it works:The form on the webpage binds to the ReceiveFridgeVm.
        //When the user submits the form, ASP.NET automatically fills this class with the input values.
        //Then, in the controller, we take this ViewModel and convert it into a Fridge object that we save to the database.
        
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
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
