using System.ComponentModel.DataAnnotations;

namespace FridgeManagementSystem.Models
{
    public class BussinessInfo
    {
<<<<<<<<< Temporary merge branch 1
        [Key]
        public int BusinnessInfoId { get; set; }

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
=========

>>>>>>>>> Temporary merge branch 2
    }
}
