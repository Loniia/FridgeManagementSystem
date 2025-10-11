using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class BusinessInfo
    {
        [Key]
        public int BusinessInfoId { get; set; }

        // Basic Company Info
        [Required, MaxLength(300)]
        public string CompanyName { get; set; }

        [MaxLength(200)]
        public string RegistrationNumber { get; set; }

        [MaxLength(100)]
        public string TaxNumber { get; set; }

        // Contact Information
        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Website { get; set; }

        // Business Description & "What We Do"
        [MaxLength(1000)]
        public string CompanyDescription { get; set; } // "About Us" section

        [MaxLength(1500)]
        public string MissionStatement { get; set; }

        [MaxLength(1500)]
        public string ServicesDescription { get; set; } // Detailed "What we do"

        [MaxLength(1000)]
        public string CoreValues { get; set; }

        // Business Operations
        [MaxLength(200)]
        public string Industry { get; set; }

        public int YearFounded { get; set; }

        [MaxLength(100)]
        public string BusinessType { get; set; } // e.g., "Beverage Distribution", "Fridge Management"

        // Visual Identity
        [MaxLength(500)]
        public string LogoUrl { get; set; }

        [MaxLength(500)]
        public string BannerImageUrl { get; set; }

        // Social Media & Additional Info
        [MaxLength(200)]
        public string FacebookUrl { get; set; }

        [MaxLength(200)]
        public string LinkedInUrl { get; set; }

        [MaxLength(200)]
        public string TwitterUrl { get; set; }

        // Timestamps
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
