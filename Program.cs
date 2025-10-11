using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

// ✅ 1. Add DbContext with SQL Server
builder.Services.AddDbContext<FridgeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<FridgeService>();


// ✅ 2. Add Identity with int as key and custom ApplicationUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<FridgeDbContext>()
    .AddDefaultTokenProviders();

// ✅ 3. Add MVC (controllers + views)
builder.Services.AddControllersWithViews();

// ✅ 4. Add ServiceHistoryPdfGenerator to DI container
builder.Services.AddScoped<IMaintenanceRequestService, MaintenanceRequestService>();

// ✅ 5. Configure role-based authorization
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";       // Login page
    options.AccessDeniedPath = "/Account/AccessDenied"; // Access denied page
});

var app = builder.Build();

// ✅ 6. Run migrations + seed data at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<FridgeDbContext>();
   // context.Database.Migrate();

    // Call SeedData to ensure roles + admin user exist
    await SeedData.InitializeAsync(services);
}

// ✅ 7. Configure Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Identity
app.UseAuthorization();

// ✅ 8. FIXED: Explicit Area Route Configuration
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
await app.RunAsync();

