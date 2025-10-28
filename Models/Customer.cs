using FridgeManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Contact info is required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Provide valid email address")]
        public string Email { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, ErrorMessage = "Phone number cannot be longer than 10 characters.")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        // ✅ Allow manual or past entry of date
        [DataType(DataType.Date)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; } // for notifications

        // ✅ Allow choosing a past registration date
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Registration Date")]
        public DateOnly RegistrationDate { get; set; }

        public ApplicationUser UserAccount { get; set; }
        public bool IsVerified { get; set; } = false;
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "ShopType is required")]
        [EnumDataType(typeof(ShopType))]
        public ShopType ShopType { get; set; }

        public int? ApplicationUserId { get; set; }

        [Required]
        [StringLength(200)]
        public string SecurityQuestion { get; set; }

        [Required]
        public string SecurityAnswerHash { get; set; }

        public ICollection<FaultReport> FaultReports { get; set; }
        public ICollection<CustomerNote> CustomerNote { get; set; }

        // Navigation properties
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<FridgeAllocation> FridgeAllocation { get; set; }
        public virtual ICollection<PurchaseRequest> PurchaseRequest { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<Fault> Faults { get; set; } = new List<Fault>();
        public virtual ICollection<Fridge> Fridge { get; set; }
        public ICollection<CustomerNotification> CustomerNotifications { get; set; }

        [NotMapped]
        public string StatusDisplay => IsActive ? "Active" : "Inactive";

        [NotMapped]
        public int TotalFridges => Fridge?.Count ?? 0;
    }
}
