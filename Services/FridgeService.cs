using FridgeManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Models
{
    public class FridgeService
    {
        private readonly FridgeDbContext _context;

        public FridgeService(FridgeDbContext context)
        {
            _context = context;
        }

        // ========== CRUD-Like ==========
        public async Task<List<Fridge>> GetAllFridgesAsync()
        {
            return await _context.Fridge
                .Include(f => f.FridgeAllocation)
                .ToListAsync();
        }

        public async Task<Fridge?> GetFridgeByIdAsync(int id)
        {
            return await _context.Fridge
                .Include(f => f.Supplier)
                .Include(f => f.Location)
                .FirstOrDefaultAsync(f => f.FridgeId == id);
        }

        public async Task<bool> CreateFridgeAsync(Fridge fridge)
        {
            fridge.Status = "Active";
            fridge.PurchaseDate = DateTime.UtcNow;

            _context.Fridge.Add(fridge);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateFridgeAsync(Fridge fridge)
        {
            if (!_context.Fridge.Any(f => f.FridgeId == fridge.FridgeId)) return false; 
             _context.Update(fridge);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFridgeAsync(int id)
        {
            var fridge = await _context.Fridge.FindAsync(id);
            if (fridge == null) return false;

            _context.Fridge.Remove(fridge);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeallocateFromCustomerAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null || customer.Fridge == null) return false;

            customer.Fridge = null;
            _context.Update(customer);
            await _context.SaveChangesAsync();

            return true;
        }

        // Note: Admin doesn't schedule maintenance. Maintenance Technician handles this.

        // ========== REPORTING ==========
        public async Task<List<InventoryReportVM>> GetInventoryReportAsync()
        {
            return await _context.Fridge
                .Include(f => f.Location)
                .GroupBy(f => f.Location.City)
                .Select(g => new InventoryReportVM
                {
                    Location = g.Key,
                    TotalFridges = g.Count(),
                    Active = g.Count(f => f.Status == "Active"),
                    InRepair = g.Count(f => f.Status == "In Repair"),
                    Retired = g.Count(f => f.Status == "Retired")
                })
                .ToListAsync();
        }

        public async Task<List<SupplierReportVM>> GetSupplierReportAsync()
        {
            return await _context.Fridge
                .Include(f => f.Supplier)
                .GroupBy(f => f.Supplier.Name)
                .Select(g => new SupplierReportVM
                {
                    SupplierName = g.Key,
                    TotalSupplied = g.Count()
                })
                .ToListAsync();
        }

        // ========== ADMIN VIEW: Fridges + Maintenance ==========
        public async Task<List<FridgeWithMaintenanceVM>> GetAllFridgesWithMaintenanceAsync()
        {
            var fridges = await _context.Fridge
                .Include(f => f.Supplier)
                .Include(f => f.Location)
                .Include(f => f.MaintenanceVisit) // Maintenance info
                .ToListAsync();

            // Map to ViewModel
            var result = fridges.Select(f => new FridgeWithMaintenanceVM
            {
                FridgeId = f.FridgeId,
                SerialNumber = f.SerialNumber,
                SupplierName = f.Supplier?.Name ?? "",
                Location = f.Location?.City ?? "",
                Status = f.Status,
                LastMaintenanceDate = f.MaintenanceVisit
        .Where(r => r.Status == TaskStatus.Complete) // Use enum here
        .OrderByDescending(r => r.ScheduledDate)
        .Select(r => (DateTime?)r.ScheduledDate)
        .FirstOrDefault(),
                NextScheduledMaintenance = f.MaintenanceVisit
        .Where(r => r.Status == TaskStatus.Scheduled) // Use enum here
        .OrderBy(r => r.ScheduledDate)
        .Select(r => (DateTime?)r.ScheduledDate)
        .FirstOrDefault()
            }).ToList();

            return result;
        }
    }

    // ===== VIEW MODELS =====
    public class InventoryReportVM
    {
        public string Location { get; set; } = "";
        public int TotalFridges { get; set; }
        public int Active { get; set; }
        public int InRepair { get; set; }
        public int Retired { get; set; }
        // Detailed fridge info
        public int FridgeId { get; set; }
        public string? FridgeName { get; set; }
        public string? FridgeType { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public string? Condition { get; set; }
        public int Quantity { get; set; }
        public string? Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string? Notes { get; set; }
        public string? SupplierName { get; set; }
        public string? CustomerName { get; set; }
        public DateOnly? AllocationDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public DateTime? LastMaintenance { get; set; }
        public DateTime? NextScheduledMaintenance { get; set; }

        // Computed properties
        public bool IsUnderWarranty => WarrantyExpiry.HasValue && WarrantyExpiry.Value > DateTime.Today;
        public bool IsLowStock => Quantity < 5;
        public bool IsActive { get; set; }
    }

    public class SupplierReportVM
    {
        public string SupplierName { get; set; } = "";
        public int TotalSupplied { get; set; }
    }

    // New ViewModel: Admin Inventory with Maintenance Info
    public class FridgeWithMaintenanceVM
    {
        public int FridgeId { get; set; }
        public string SerialNumber { get; set; } = "";
        public string SupplierName { get; set; } = "";
        public string Location { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextScheduledMaintenance { get; set; }
    }
}