using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace FridgeManagementSystem.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Ensure roles exist
            string[] roles = { Roles.Admin, Roles.Customer, Roles.Employee };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }

            // Ensure Admin user exists
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@fridge.com",
                    FullName = "System Admin",
                    UserType = Roles.Admin,   // match Roles.Admin
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);  // match Roles.Admin
                }
            }
        }
    }
}
