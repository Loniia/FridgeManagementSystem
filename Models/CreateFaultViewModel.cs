using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace FridgeManagementSystem.Models
{
    // Create a ViewModel for the CreateFault view
    public class CreateFaultViewModel
    {
        [Required(ErrorMessage = "Please select a fridge")]
        [Display(Name = "Fridge")]
        public int FridgeId { get; set; }

        [Required(ErrorMessage = "Please select priority")]
        public string Priority { get; set; }

        [Required(ErrorMessage = "Please describe the fault")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Fault Description")]
        public string FaultDescription { get; set; }

        // CHANGE THESE TO List<SelectListItem>
        public List<SelectListItem> FridgeOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PriorityOptions { get; set; } = new List<SelectListItem>();
    }
}
