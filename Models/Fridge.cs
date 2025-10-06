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

        [Required(ErrorMessage = "Fridge Type is required")]
        [StringLength(100, ErrorMessage = "Fridge Type cannot exceed 100 characters")]
        public string FridgeType { get; set; }
        public string Brand { get; set; }
        [Required(ErrorMessage = "Model is required")]
        [StringLength(50, ErrorMessage = "Model cannot exceed 50 characters")]
        public string Model { get; set; }
        public string SerialNumber {  get; set; }
        [Required]
        [Display(Name = "Condition")]
        public string Condition { get; set; } = "Working"; // Working, Under Repair, Faulty, Scrapped

        public DateTime PurchaseDate { get; set; }

       

        [Display(Name = "Warranty Expiry")]
        [DataType(DataType.Date)]
        public DateTime? WarrantyExpiry { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public DateOnly DateAdded { get; set; }= DateOnly.FromDateTime(DateTime.Now);
        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerId {  get; set; }
        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location Location { get; set; }  
        public int FaultID { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; } // e.g., Received, In Transit, Delivered
        public bool IsActive { get; set; } = true;
        public string ImageUrl { get; set; } // for pictures
        public decimal Price { get; set; }

        //Navigation Property
        public virtual ICollection<FridgeAllocation> FridgeAllocation { get; set; }
        public virtual Inventory Inventories { get; set; }
        public virtual ScrappedFridge ScrappedFridges { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<MaintenanceRequest> MaintenanceRequest { get; set; }
        public virtual ICollection<MaintenanceVisit> MaintenanceVisit { get; set; }
    
        public virtual ICollection<FaultReport> FaultReport { get; set; }
        public ICollection<Fault> Faults { get; set; }

        [NotMapped]
        public bool IsUnderWarranty
        {
            get
            {
                return WarrantyExpiry.HasValue && WarrantyExpiry.Value > DateTime.Today;
            }
        }

        [NotMapped]
        public int ActiveFaultsCount => Faults?.Count(f => f.Status != "Resolved") ?? 0;
    }
}