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
                .Include(f => f.Supplier)
                .Include(f => f.Location)
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

        public async Task<bool> ScheduleMaintenanceAsync(int fridgeId, int employeeId, DateTime scheduledDate)
        {
            var fridge = await _context.Fridge.FindAsync(fridgeId);
            var employee = await _context.Employees.FindAsync(employeeId);

            if (fridge == null || employee == null) return false;
            if (fridge.Status == "In Repair") return false;

            fridge.Status = "In Repair";
            _context.Update(fridge);

            var schedule = new RepairSchedule
            {
                FridgeId = fridgeId,
                EmployeeId = employeeId,
                RepairDate = scheduledDate,
                Status = "Scheduled"
            };
            _context.RepairSchedules.Add(schedule);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteMaintenanceAsync(int fridgeId, int scheduleId)
        {
            var fridge = await _context.Fridge.FindAsync(fridgeId);
            var schedule = await _context.RepairSchedules.FindAsync(scheduleId);

            if (fridge == null || schedule == null) return false;

            fridge.Status = "Active";
            schedule.Status = "Completed";

            _context.Update(fridge);
            _context.Update(schedule);
            await _context.SaveChangesAsync();

            return true;
        }

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
    }

    // ViewModels
    public class InventoryReportVM
    {
        public string Location { get; set; } = "";
        public int TotalFridges { get; set; }
        public int Active { get; set; }
        public int InRepair { get; set; }
        public int Retired { get; set; }
    }

    public class SupplierReportVM
    {
        public string SupplierName { get; set; } = "";
        public int TotalSupplied { get; set; }
    }
}
