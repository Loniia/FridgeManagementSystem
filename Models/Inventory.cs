using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryID { get; set; }

        [ForeignKey("Fridge")]
        public int FridgeID { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be zero or more")]
        public int Quantity { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? LastUpdated { get; set; } // ✅ made nullable, removed auto "Now"

        // Navigation Properties
        public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; }
        public virtual Fridge Fridge { get; set; }
    }
}
