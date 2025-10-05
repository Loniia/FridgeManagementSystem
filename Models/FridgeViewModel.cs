using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
namespace FridgeManagementSystem.Models
#nullable disable
{
    public class FridgeViewModel
    {
        public int FridgeId { get; set; }
        public string FridgeName { get; set; }
        public string FridgeType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string Condition { get; set; } = "Working";
        public DateTime PurchaseDate { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public DateOnly DateAdded { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public int SupplierID { get; set; }
        public int? CustomerId { get; set; }
        public int? LocationId { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; } = true;
        public string ImageUrl { get; set; } // for pictures
        public decimal Price { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string LocationName { get; set; }

        // Allocation info
        public DateOnly? AllocationDate { get; set; }
        public DateOnly? ReturnDate { get; set; }

        // Computed properties
        [NotMapped]
        public bool IsUnderWarranty => WarrantyExpiry.HasValue && WarrantyExpiry.Value > DateTime.Today;

        [NotMapped]
        public bool IsLowStock => Quantity < 5;
    }
}
