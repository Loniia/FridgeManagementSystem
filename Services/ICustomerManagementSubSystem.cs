using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Services
{
    public interface ICustomerManagementSubSystem
    {
        Task<IEnumerable<Fridge>> GetAllFridgesAsync();
        Task<Fridge?> GetFridgeByIdAsync(int fridgeId);
        Task RequestFridgeAsync(int fridgeId);
    }

    public class CustomerManagementService : ICustomerManagementSubSystem
    {
        private readonly FridgeDbContext _context;

        public CustomerManagementService(FridgeDbContext context)
        {
            _context = context;
        }

        // ✅ Get all fridges from the database
        public async Task<IEnumerable<Fridge>> GetAllFridgesAsync()
        {
            return await _context.Fridge
                .Where(f => f.IsActive) // optional filter
                .ToListAsync();
        }

        // ✅ Find a fridge by its ID
        public async Task<Fridge?> GetFridgeByIdAsync(int fridgeId)
        {
            return await _context.Fridge.FindAsync(fridgeId);
        }

        // ✅ Update fridge status to "Requested"
        public async Task RequestFridgeAsync(int fridgeId)
        {
            var fridge = await _context.Fridge.FindAsync(fridgeId);
            if (fridge != null)
            {
                fridge.Status = "Requested";
                await _context.SaveChangesAsync();
            }
        }
    }
}



