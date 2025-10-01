using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
#nullable disable
namespace FridgeManagementSystem.ViewModels
{
    public class AllocateFridgeViewModel
    {
        public int FridgeId { get; set; }
        public string FridgeSerialNumber { get; set; }
        public virtual Fridge Fridge { get; set; }

        // Selected Customer
        public int CustomerId { get; set; }

        // Dropdown list of customers
        public List<SelectListItem> Customers { get; set; } = new();
    }
}
