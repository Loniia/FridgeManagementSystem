using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FridgeManagementSystem.Data;

namespace FridgeManagementSystem.Models
#nullable disable
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; } 

        [Required(ErrorMessage = "Supplier Name is required.")]
        [StringLength(100, ErrorMessage = "Supplier Name cannot be longer than 100 characters.")] 
        [Display(Name = "Supplier Name")]
        public string Name { get; set; } 

        [Required]
        [StringLength(50, ErrorMessage = "Contact Person cannot be longer than 50 characters.")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")] 
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters.")] 
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } 

        
        [Required(ErrorMessage = "Address is required for delivery and invoicing.")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }
        public int PurchaseOrderID { get; set; }
        public int QuotationID { get; set; }
        public int FridgeId { get; set; }
       
        // Soft delete flag
        public bool IsActive { get; set; } = true;

        // Navigation
        public virtual ICollection<Fridge> Fridges { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        public virtual ICollection<Quotation> Quotations { get; set; }
        public virtual ICollection<Fridge> Fridges { get; set; }
    }

}


