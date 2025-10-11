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
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
         ErrorMessage = "Provide valid email address")]
        public string Email { get; set; }
 
        [ForeignKey("Location")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, ErrorMessage = "Phone number cannot be longer than 10 characters.")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } //for the notification

        [Required]
        [DataType(DataType.Date)]
        public DateOnly RegistrationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        
        public ApplicationUser UserAccount { get; set; }
        public bool IsVerified { get; set; } = false;     // Admin must verify
        public bool IsActive { get; set; } = true;

        //Added by Idah
        //shebeen,Spaza,Restaurant,Supermarket 
        [Required(ErrorMessage = "ShopType is required")]
        [EnumDataType(typeof(ShopType))]
        public ShopType ShopType { get; set; }

        //When you delete a user, their Customer profile is also deleted.
        public int? ApplicationUserId { get; set; }
        [Required]
        [StringLength(200)]
        public string SecurityQuestion { get; set; }

        [Required]
        public string SecurityAnswerHash { get; set; }
        public ICollection<FaultReport> FaultReports { get; set; }
       
        public ICollection<CustomerNote> CustomerNote { get; set; }

        //Navigation Property
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<FridgeAllocation> FridgeAllocation { get; set; }
        public virtual ICollection<PurchaseRequest> PurchaseRequest { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<Fault> Faults { get; set; } = new List<Fault>();
        public virtual ICollection<Fridge> Fridge { get; set; }
        //For being rejected 
        public ICollection<CustomerNotification> CustomerNotifications { get; set; }
        //public virtual ICollection<BussinessInfo> BussinessInfo { get; set; }

        // Add computed property for display
        [NotMapped] // This won't be stored in database
        public string StatusDisplay => IsActive ? "Active" : "Inactive";

        [NotMapped]
        public int TotalFridges => Fridge?.Count ?? 0;
    }
}
