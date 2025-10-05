using System.ComponentModel.DataAnnotations;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class Location
    {
        [Key] 
        public int LocationId { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Province is required")]
        public string Province { get; set; }
        [Required(ErrorMessage = "Postal code is required")]
        public string PostalCode { get; set; }
        // Soft delete flag
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<Fridge> Fridge { get; set; }
    }

}

