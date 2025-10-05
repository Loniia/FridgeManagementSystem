using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class Employee
    {

        [Key]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        public string PhoneNumber{ get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        [Required]
        [StringLength(50)]
        public string Role { get; set; } // Example: "Technician", "Customer Liaison", etc.
        [Required]
        
        public DateTime HireDate { get; set; }
      
        public decimal PayRate { get; set; } // Example: "Technician", "Customer Liaison", etc.

        [Required]
        [RegularExpression("^(Active|Inactive)$",
            ErrorMessage = "Status must be 'Active' or 'Inactive'")]
        public string Status { get; set; } = "Active";

        
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        [ForeignKey("ApplicationUserId")]
        
        public int ApplicationUserId { get; set; }
        public ApplicationUser UserAccount { get; set; }

        // --------------------------------------------------------------------------------
        // Not mapped fields so we can bind password fields directly on the Employee model
        // --------------------------------------------------------------------------------
        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public virtual ICollection<Fault> Faults { get; set; } = new List<Fault>();
    }
}
