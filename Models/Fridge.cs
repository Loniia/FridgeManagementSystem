using FridgeManagementSystem.Models;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeManagementSystem.Models
#nullable disable
{
    public class Fridge
    {
        public int FridgeId { get; set; }



        [Required(ErrorMessage = "Fridge name is required")]
        [StringLength(100, ErrorMessage = "Fridge name cannot exceed 100 characters")]

        public string FridgeName { get; set; }
        public string FridgeType { get; set; }
        public string Brand { get; set; }
        [Required(ErrorMessage = "Model is required")]
        [StringLength(50, ErrorMessage = "Model cannot exceed 50 characters")]
        public string Model { get; set; }
        public string Type { get; set; }
        public string SerialNumber {  get; set; }
        public DateOnly DateAdded { get; set; }= DateOnly.FromDateTime(DateTime.Now);
        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId {  get; set; }
        public int FaultID { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; } // e.g., Received, In Transit, Delivered
        public bool IsActive { get; set; } = true;

        //Navigation Property
        public virtual ICollection<FridgeAllocation> FridgeAllocation { get; set; }
        public virtual Inventory Inventories { get; set; }
        public virtual ScrappedFridge ScrappedFridges { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<MaintenanceRequest> MaintenanceRequest { get; set; }
        public virtual ICollection<MaintenanceVisit> MaintenanceVisit { get; set; }

        public virtual ICollection<FaultReport> FaultReport { get; set; }
        public ICollection<Fault> Fault { get; set; }
    }
}