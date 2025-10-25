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
            var context = serviceProvider.GetRequiredService<FridgeDbContext>(); // ✅ Get DB context

            // ---------------------------
            // 🌟 Ensure roles exist
            // ---------------------------
            string[] roles = { Roles.Admin, Roles.Customer, Roles.Employee };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }

            // ---------------------------
            // 🌟 Ensure Admin user exists
            // ---------------------------
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

            // ---------------------------
            // 🌟 Seed Fridges if not exist
            // ---------------------------
            if (!context.Fridge.Any())
            {
                var rnd = new Random();
                var brands = new[] { "LG", "Samsung", "Bosch", "Hisense", "Defy" };
                var types = new[] { "Single Door", "Double Door", "Side-by-Side", "Mini Fridge" };

                var fridges = new List<Fridge>();

                for (int i = 1; i <= 30; i++)
                {
                    fridges.Add(new Fridge
                    {
                        FridgeType = types[rnd.Next(types.Length)],
                        Brand = brands[rnd.Next(brands.Length)],
                        Model = $"Model-{i}",
                        Condition = "Working",
                        SupplierID = 1,
                        Price = rnd.Next(3500, 12000),  // ✅ Random price between R3500 and R12000
                        ImageUrl = $"/images/fridges/fridge{i}.jpg",
                        IsActive = true,
                        Quantity = 0,          // Start as out of stock
                        Status = "Pending",   // Default status
                        DeliveryDate = DateTime.Now
                    });
                }

                context.Fridge.AddRange(fridges);
                await context.SaveChangesAsync();
            }
        }
    }
}


//using FridgeManagementSystem.Models;
//using Microsoft.AspNetCore.Identity;

//namespace FridgeManagementSystem.Data
//{
//    public static class SeedData
//    {
//        public static async Task InitializeAsync(IServiceProvider serviceProvider)
//        {
//            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
//            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//            // Ensure roles exist
//            string[] roles = { Roles.Admin, Roles.Customer, Roles.Employee };
//            foreach (var role in roles)
//            {
//                if (!await roleManager.RoleExistsAsync(role))
//                {
//                    await roleManager.CreateAsync(new IdentityRole<int>(role));
//                }
//            }

//            // Ensure Admin user exists
//            var adminUser = await userManager.FindByNameAsync("admin");
//            if (adminUser == null)
//            {
//                var user = new ApplicationUser
//                {
//                    UserName = "admin",
//                    Email = "admin@fridge.com",
//                    FullName = "System Admin",
//                    UserType = Roles.Admin,   // match Roles.Admin
//                    EmailConfirmed = true
//                };

//                var result = await userManager.CreateAsync(user, "Admin@123");
//                if (result.Succeeded)
//                {
//                    await userManager.AddToRoleAsync(user, Roles.Admin);  // match Roles.Admin
//                }
//            }
//        }
//    }
//}
