using Microsoft.AspNetCore.Identity;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }

        // "Admin", "Employee", or "Customer"
        public string UserType { get; set; }

        public string EmployeeRole { get; set; }

        // Optional: Navigation property
        public Employee EmployeeProfile { get; set; }
        public Customer CustomerProfile { get; set; }
    }
}
