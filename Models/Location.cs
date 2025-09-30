using System.ComponentModel.DataAnnotations;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class Location
    {
        [Key] 
        public int LocationId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }    
        public string Province { get; set; }
        public string PostalCode { get; set; }
        // Soft delete flag
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<Fridge> Fridge { get; set; }
    }

}

