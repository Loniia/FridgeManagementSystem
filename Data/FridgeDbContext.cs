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

            builder.Entity<Customer>()
                .HasMany(c => c.PurchaseRequest)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerID)
                .OnDelete(DeleteBehavior.NoAction);

            // Fridge Relationships (Consolidated)
            builder.Entity<Fridge>()
                .HasMany(f => f.Fault)
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
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

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

            // Supplier unique constraint (if needed)
            builder.Entity<Fridge>()
                .HasIndex(f => f.SupplierID)
                .IsUnique();

            // --- Soft Delete / Query Filters ---
            builder.Entity<Supplier>().HasQueryFilter(s => s.IsActive);
            builder.Entity<Customer>().HasQueryFilter(c => c.IsActive);
            builder.Entity<Fridge>().HasQueryFilter(f => f.IsActive);
            builder.Entity<FridgeAllocation>().HasQueryFilter(a => a.Fridge.IsActive);
            builder.Entity<Inventory>().HasQueryFilter(i => i.Fridge.IsActive);
            builder.Entity<ScrappedFridge>().HasQueryFilter(s => s.Fridge.IsActive);
            builder.Entity<PurchaseRequest>().HasQueryFilter(p => p.Customer.IsActive);
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