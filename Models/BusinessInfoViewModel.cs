using System.ComponentModel.DataAnnotations;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class BusinessInfoViewModel
    {
        public int BusinessInfoId { get; set; }

        // Basic Company Info
        [Required(ErrorMessage = "Company name is required")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Display(Name = "Tax Number")]
        public string TaxNumber { get; set; }

        // Contact Information
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Url(ErrorMessage = "Please enter a valid website URL")]
        [Display(Name = "Website")]
        public string Website { get; set; }

        // Business Description
        [Display(Name = "Company Description")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string CompanyDescription { get; set; }

        [Display(Name = "Mission Statement")]
        [StringLength(1500, ErrorMessage = "Mission statement cannot exceed 1500 characters")]
        public string MissionStatement { get; set; }

        [Display(Name = "What We Do / Services")]
        [StringLength(1500, ErrorMessage = "Services description cannot exceed 1500 characters")]
        public string ServicesDescription { get; set; }

        [Display(Name = "Core Values")]
        [StringLength(1000, ErrorMessage = "Core values cannot exceed 1000 characters")]
        public string CoreValues { get; set; }

        // Business Operations
        [Display(Name = "Industry")]
        public string Industry { get; set; }

        [Display(Name = "Year Founded")]
        [Range(1900, 2100, ErrorMessage = "Please enter a valid year")]
        public int YearFounded { get; set; }

        [Display(Name = "Business Type")]
        public string BusinessType { get; set; }

        // Visual Identity
        [Display(Name = "Logo")]
        public string LogoUrl { get; set; }

        [Display(Name = "Banner Image")]
        public string BannerImageUrl { get; set; }

        // Social Media
        [Url(ErrorMessage = "Please enter a valid URL")]
        [Display(Name = "Facebook Page")]
        public string FacebookUrl { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        [Display(Name = "LinkedIn Profile")]
        public string LinkedInUrl { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        [Display(Name = "Twitter Profile")]
        public string TwitterUrl { get; set; }

        // File Uploads
        [Display(Name = "Upload Logo")]
        public IFormFile LogoFile { get; set; }

        [Display(Name = "Upload Banner Image")]
        public IFormFile BannerFile { get; set; }
    }
}
