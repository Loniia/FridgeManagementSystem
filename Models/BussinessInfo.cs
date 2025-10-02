using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeManagementSystem.Models
{
    public class BussinessInfo
    {

        [Key]
        public int BusinessInfoId { get; set; }

        [Required, MaxLength(300)]
        public string CompanyName { get; set; }

        [MaxLength(200)]
        public string RegistrationNumber { get; set; }

        [MaxLength(100)]
        public string TaxNumber { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(500)]
        public string LogoUrl { get; set; }
        [ForeignKey("Customer")]
        public int CustomerID {  get; set; }
         public virtual Customer Customer { get; set; }

    }
}
