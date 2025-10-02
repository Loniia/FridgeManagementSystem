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
        //public DbSet<PurchaseRequestItem> PurchaseRequestItems { get; set; }
        public DbSet<RequestForQuotation> RequestsForQuotation { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<DeliveryNote> DeliveryNotes { get; set; }

        // Add any other tables as you grow (like Fridges, Suppliers, etc.)

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
                .HasForeignKey<Inventory>(i => i.FridgeID);

            builder.Entity<Fridge>()
                .HasOne(f => f.ScrappedFridges)
                .WithOne(s => s.Fridge)
                .HasForeignKey<ScrappedFridge>(s => s.FridgeID);

            // --- 1-to-many relationships ---
            builder.Entity<Fridge>()
                .HasMany(f => f.FridgeAllocation)
                .WithOne(a => a.Fridge)
                .HasForeignKey(a => a.FridgeId);

            builder.Entity<Customer>()
                .HasMany(c => c.PurchaseRequest)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerID);

            // RepairSchedule -> Fault (1-to-many)
            builder.Entity<RepairSchedule>()
                .HasOne(r => r.Fault)
                .WithMany(f => f.RepairSchedules)
                .HasForeignKey(r => r.FaultID)
                .OnDelete(DeleteBehavior.Restrict);

            // Fridge -> Fault (1-to-many)
            builder.Entity<Fridge>()
                .HasMany(f => f.Fault)
                .WithOne(fa => fa.Fridge)
                .HasForeignKey(fa => fa.FridgeId)
                .OnDelete(DeleteBehavior.Restrict);

            // FaultReport -> Fridge (many-to-1)
            builder.Entity<FaultReport>()
                .HasOne(fr => fr.Fridge)
                .WithMany(f => f.FaultReport)
                .HasForeignKey(fr => fr.FridgeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Fridge: unique SerialNumber
            builder.Entity<Fridge>()
                .HasIndex(f => f.SerialNumber)
                .IsUnique();

            // RepairSchedule -> Fridge (many-to-1)
            builder.Entity<RepairSchedule>()
                .HasOne(rs => rs.Fridge)
                .WithMany()
                .HasForeignKey(rs => rs.FridgeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Customer>()
                .HasMany(c => c.Fridge)
                .WithOne(f => f.Customer)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.SetNull); // Customer deleted → fridge unallocated

            //// Supplier ↔ Fridge (One-to-Many)
            //builder.Entity<Supplier>()
            //    .HasMany(s => s.Fridges)
            //    .WithOne(f => f.Supplier)
            //    .HasForeignKey(f => f.SupplierID)
            //    .OnDelete(DeleteBehavior.Restrict); // don’t auto-delete fridges

            // Location ↔ Fridge (One-to-Many)
            builder.Entity<Location>()
                .HasMany(l => l.Fridge)
                .WithOne(f => f.Location)
                .HasForeignKey(f => f.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Fridge ↔ FaultReport (One-to-Many) → already defined above

            // --- Soft Delete / Query Filters ---
            builder.Entity<Supplier>().HasQueryFilter(s => s.IsActive);
            builder.Entity<Customer>().HasQueryFilter(c => c.IsActive);
            builder.Entity<Fridge>().HasQueryFilter(f => f.IsActive);
            builder.Entity<FridgeAllocation>().HasQueryFilter(a => a.Fridge.IsActive);
            builder.Entity<Inventory>().HasQueryFilter(i => i.Fridge.IsActive);
            builder.Entity<ScrappedFridge>().HasQueryFilter(s => s.Fridge.IsActive);
            builder.Entity<PurchaseRequest>().HasQueryFilter(p => p.Customer.IsActive);
            builder.Entity<FaultReport>().HasQueryFilter(fr => fr.Fridge.IsActive);
            // builder.Entity<RepairSchedule>().HasQueryFilter(rs => rs.Customer.IsActive);

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

            // ✅ Configure relationships with NoAction only where not explicitly set
            foreach (var relationship in builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.ClientCascade))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

        }
    }
}
