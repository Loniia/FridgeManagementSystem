using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeManagementSystem.Models
#nullable disable
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        [Required(ErrorMessage = "Supplier Name is required.")]
        [StringLength(50, ErrorMessage = "Supplier Name cannot be longer than 50 characters.")]
        public string SupplierName { get; set; }

         [Required]
         [StringLength(20)]
         public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(30, ErrorMessage = "Email cannot be longer than 30 characters.")]
        public string Email { get; set; }
       
        [Required(ErrorMessage ="Phone number is required.")]
        [StringLength(10, ErrorMessage ="Phone number cannot be longer than 10 characters.")]
        [Phone(ErrorMessage ="Invalid Phone Number")]
        public string PhoneNumber { get; set; }
        [ForeignKey("Location")]
        public int LocationId {  get; set; }
        
        public bool IsActive { get; set; } = true;
        
        //Navigation Property
        //public virtual ICollection<Fridge> Fridge { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get;set; }
    }

}


