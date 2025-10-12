using FridgeManagementSystem.Models; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
namespace FridgeManagementSystem.Data
{
    public class FridgeDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public FridgeDbContext(DbContextOptions<FridgeDbContext> options)
            : base(options)
        {
        }

        // Core Tables
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Fridge> Fridge { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Fault> Faults { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<FridgeAllocation> FridgeAllocation { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<ScrappedFridge> ScrappedFridges { get; set; }
        public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
        public DbSet<FaultReport> FaultReport { get; set; }
        public DbSet<MaintenanceChecklist> MaintenanceChecklist { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequest { get; set; }
        public DbSet<MaintenanceVisit> MaintenanceVisit { get; set; }
        public DbSet<ComponentUsed> ComponentUsed { get; set; }
        public DbSet<RepairSchedule> RepairSchedules { get; set; }
        public DbSet<RequestForQuotation> RequestsForQuotation { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<DeliveryNote> DeliveryNotes { get; set; }
        // Customer E-Commerce Tables by Idah
        //public DbSet<FinancialAccount> FinancialAccounts { get; set; }
        //public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CustomerNotification> CustomerNotifications { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<AdminNotification> AdminNotifications { get; set; }
        public DbSet<BusinessInfo> BusinessInfos { get; set; }

        public DbSet<Notification> Notifications { get; set; }



        // --------------------------
        // Calculate Available Stock
        // --------------------------
        public int CalculateAvailableStock(Fridge fridge)
        {
            if (fridge.FridgeAllocation == null || !fridge.FridgeAllocation.Any())
                return fridge.Quantity;

            var allocatedCount = fridge.FridgeAllocation
                .Where(a => a.ReturnDate == null || a.ReturnDate > DateOnly.FromDateTime(DateTime.Today))
                .Count();

            return fridge.Quantity - allocatedCount;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 🔗 ApplicationUser to Employee (One-to-One)
            builder.Entity<ApplicationUser>()
                .HasOne(a => a.EmployeeProfile)
                .WithOne(e => e.UserAccount)
                .HasForeignKey<Employee>(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔗 ApplicationUser to Customer (One-to-One)
            builder.Entity<ApplicationUser>()
                .HasOne(a => a.CustomerProfile)
                .WithOne(c => c.UserAccount)
                .HasForeignKey<Customer>(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- 1-to-1 relationships ---
            builder.Entity<Fridge>()
                .HasOne(f => f.Inventories)
                .WithOne(i => i.Fridge)
                .HasForeignKey<Inventory>(i => i.FridgeID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Fridge>()
                .HasOne(f => f.ScrappedFridges)
                .WithOne(s => s.Fridge)
                .HasForeignKey<ScrappedFridge>(s => s.FridgeID)
                .OnDelete(DeleteBehavior.NoAction);

            // --- 1-to-many relationships ---
            builder.Entity<Fridge>()
                .HasMany(f => f.FridgeAllocation)
                .WithOne(a => a.Fridge)
                .HasForeignKey(a => a.FridgeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Fridge Relationships (Consolidated)
            builder.Entity<Fridge>()
                .HasMany(f => f.Faults)
                .WithOne(f => f.Fridge)
                .HasForeignKey(f => f.FridgeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Fridge>()
                .HasMany(f => f.FaultReport)
                .WithOne(fr => fr.Fridge)
                .HasForeignKey(fr => fr.FridgeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Customer -> Fridge
            builder.Entity<Customer>()
                .HasMany(c => c.Fridge)
                .WithOne(f => f.Customer)
                .HasForeignKey(f => f.CustomerID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<CustomerNotification>()
                .HasOne(n => n.Customer)           // Each notification belongs to one customer
                .WithMany(c => c.CustomerNotifications) // Each customer can have many notifications
                .HasForeignKey(n => n.CustomerId) // Foreign key in CustomerNotification
                .OnDelete(DeleteBehavior.Cascade); // Deletes notifications if customer is deleted


            // Location -> Fridge (FIXED - NoAction to prevent cascade cycles)
            builder.Entity<Fridge>()
                 .HasOne(f => f.Location)
                 .WithMany(l => l.Fridge)
                 .HasForeignKey(f => f.LocationId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .IsRequired(false);

            // RepairSchedule relationships
            builder.Entity<RepairSchedule>()
                .HasOne(r => r.Fault)
                .WithMany(f => f.RepairSchedules)
                .HasForeignKey(r => r.FaultID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RepairSchedule>()
                .HasOne(rs => rs.Fridge)
                .WithMany()
                .HasForeignKey(rs => rs.FridgeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Fridge: unique SerialNumber
            builder.Entity<Fridge>()
                .HasIndex(f => f.SerialNumber)
                .IsUnique();

            builder.Entity<Fault>()
                .HasOne(f => f.Fridge)
                .WithMany(fr => fr.Faults)
                .HasForeignKey(f => f.FridgeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Supplier → Fridge: One-to-Many
            builder.Entity<Fridge>()
                .HasOne(f => f.Supplier)         // Each fridge has one supplier
                .WithMany(s => s.Fridges)       // Each supplier can have many fridges
                .HasForeignKey(f => f.SupplierID)
                .OnDelete(DeleteBehavior.Restrict);

           

            // --- Fridge -> Review (1-to-many) ---
            builder.Entity<Fridge>()
                .HasMany(f => f.Reviews)
                .WithOne(r => r.Fridge)
                .HasForeignKey(r => r.FridgeId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-one relationship between Customer and Cart
            builder.Entity<Customer>()
                .HasOne(c => c.Cart)
                .WithOne(ca => ca.Customer)
                .HasForeignKey<Cart>(ca => ca.CustomerID)

                .OnDelete(DeleteBehavior.Cascade);

            // --- Cart -> CartItem (1-to-many) ---
            builder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Customer -> Order (1-to-many) ---
            builder.Entity<Customer>()
                .HasMany<Order>()
                .WithOne()
                .HasForeignKey(o => o.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Order -> OrderItem (1-to-many) ---
            builder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Order -> Payment (1-to-1 or 1-to-many) ---
            builder.Entity<Order>()
                .HasOne<Payment>()
                .WithOne()
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

           

            // --- 🌟 Seed Supplier 🌟 ---
            builder.Entity<Supplier>().HasData(
                new Supplier
                {
                    SupplierID = 1,
                    Name = "Default Supplier",
                    Email = "supplier@example.com",
                    Phone = "0123456789",
                    Address = "123 Main Street"
                }
                
            );
            // Seed fridges
            var fridges = new List<Fridge>();
            int id = 1;
            var rnd = new Random();
            string[] brands = { "Samsung", "LG", "Hisense", "Defy", "Bosch" };
            string[] types = { "Single Door", "Double Door", "Mini Fridge" };
            for (int i = 1; i <= 12; i++)
            {
                fridges.Add(new Fridge
                {
                    FridgeId = id++,
                    FridgeType = types[rnd.Next(types.Length)],
                    Brand = brands[rnd.Next(brands.Length)],
                    Model = $"Model-{i}",
                    Condition = "Working",
                    SupplierID = 1,
                    Price = rnd.Next(3500, 12000),
                    ImageUrl = $"/images/fridges/fridge{i}.jpg",
                    IsActive = true,
                    Quantity = rnd.Next(1, 10),
                    Status = "Available",
                    DeliveryDate = DateTime.Now
                });
            }

            builder.Entity<Fridge>().HasData(fridges);
        
        // --- Soft Delete / Query Filters ---
        builder.Entity<Supplier>().HasQueryFilter(s => s.IsActive);
            builder.Entity<Customer>().HasQueryFilter(c => c.IsActive);
            builder.Entity<Fridge>().HasQueryFilter(f => f.IsActive);
            builder.Entity<FridgeAllocation>().HasQueryFilter(a => a.Fridge.IsActive);
            builder.Entity<Inventory>().HasQueryFilter(i => i.Fridge.IsActive);
            builder.Entity<ScrappedFridge>().HasQueryFilter(s => s.Fridge.IsActive);
            builder.Entity<FaultReport>().HasQueryFilter(fr => fr.Fridge.IsActive);

            // Configure enum conversions
            builder.Entity<MaintenanceRequest>()
                .Property(r => r.TaskStatus)
                .HasConversion<string>();

            builder.Entity<MaintenanceVisit>()
                .Property(v => v.Status)
                .HasConversion<string>();

            builder.Entity<MaintenanceChecklist>()
                .Property(c => c.TemperatureStatus)
                .HasConversion<string>();

            builder.Entity<MaintenanceChecklist>()
                .Property(c => c.CoolantLevel)
                .HasConversion<string>();

            builder.Entity<MaintenanceChecklist>()
                .Property(c => c.DoorSealCondition)
                .HasConversion<string>();

            builder.Entity<MaintenanceChecklist>()
                .Property(c => c.LightingStatus)
                .HasConversion<string>();

            builder.Entity<MaintenanceChecklist>()
                .Property(c => c.PowerCableCondition)
                .HasConversion<string>();

            builder.Entity<ComponentUsed>()
                .Property(c => c.Condition)
                .HasConversion<string>();

            builder.Entity<FaultReport>()
                .Property(f => f.FaultType)
                .HasConversion<string>();
            builder.Entity<Payment>()
                .Property(p => p.Method)
                .HasConversion<string>();

            // Configure relationships
            builder.Entity<Fridge>()
                .HasOne(f => f.Supplier)
                .WithMany(s => s.Fridges)
                .HasForeignKey(f => f.SupplierID);

            builder.Entity<Fridge>()
                .HasOne(f => f.Customer)
                .WithMany(c => c.Fridge)
                .HasForeignKey(f => f.CustomerID)
                .IsRequired(false);

            builder.Entity<Fridge>()
                .HasOne(f => f.Location)
                .WithMany(l => l.Fridge)
                .HasForeignKey(f => f.LocationId)
                .IsRequired(false);


            // Set default delete behavior to NoAction for any unspecified relationships
            foreach (var relationship in builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }
    }
}
