using FridgeManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace FridgeManagementSystem.Models
#nullable disable
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string FullNames { get; set; }
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public string StatusDisplay => IsActive ? "Active" : "Inactive";

        //Navigation Property
        public List<FridgeViewModel> AvailableFridges { get; set; }
        public List<FridgeAllocationViewModel> FridgeAllocations { get; set; } = new List<FridgeAllocationViewModel>();
        public int TotalFridgesAllocated => FridgeAllocations?.Sum(a => a.QuantityAllocated) ?? 0;
    }

}
