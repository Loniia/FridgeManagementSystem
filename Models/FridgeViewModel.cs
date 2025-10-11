using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
namespace FridgeManagementSystem.Models
#nullable disable
{
    public class FridgeViewModel
    {
        public int FridgeId { get; set; }
        public string FridgeType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string Condition { get; set; } = "Working";
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateOnly DateAdded { get; set; }
        public int Quantity { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; } = true;
        public string ImageUrl { get; set; } // e.g. "/images/fridges/fridge1.jpg"
        public decimal Price { get; set; }
        public int SupplierID { get; set; }
        public string CustomerName { get; set; }
        public int? LocationId { get; set; }
        public int? CustomerID { get; set; }
        public DateOnly? AllocationDate { get; set; }
        public DateOnly? ReturnDate { get; set; }

        // Computed / UI fields
        public int AvailableStock { get; set; }

        // convenience flags
        public bool IsUnderWarranty => WarrantyExpiry.HasValue && WarrantyExpiry.Value > DateTime.Today;
        public bool IsLowStock => AvailableStock < 5;
    }
}
