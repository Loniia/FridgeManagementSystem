using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Services
{
    public class CustomerService
    {
        private readonly FridgeDbContext _context;

        public CustomerService(FridgeDbContext context)
        {
            _context = context;
        }

        // --------------------------
        // Get ALL customers with allocations, notes, faults
        // --------------------------
        public async Task<List<Customer>> GetAllCustomersWithFridgesAsync()
        {
            return await _context.Customers
                .Include(c => c.FridgeAllocation)
                    .ThenInclude(fa => fa.Fridge)
                .Include(c => c.FaultReports)
                .Include(c => c.CustomerNote)
                .ToListAsync();
        }

        // --------------------------
        // Get ONE customer with details
        // --------------------------
        public async Task<Customer?> GetCustomerDetailsAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.FridgeAllocation)
                    .ThenInclude(fa => fa.Fridge)
                .Include(c => c.FaultReports)
                .Include(c => c.CustomerNote)
                .FirstOrDefaultAsync(c => c.CustomerID == id);
        }

        // --------------------------
        // Add new customer
        // --------------------------
        public async Task AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        // --------------------------
        // Update existing customer
        // --------------------------
        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        // --------------------------
        // Delete customer
        // --------------------------
        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}


