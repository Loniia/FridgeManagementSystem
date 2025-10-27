using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Services
{
    public interface ICustomerManagementReportService
    {
        // These methods return your existing entity classes, no new DTOs
        Task<Dictionary<string, object>> GetCustomerManagementDashboardAsync();
        Task<List<Customer>> GetCustomerReportAsync();
        Task<List<FridgeAllocation>> GetAllocationReportAsync(DateOnly? fromDate, DateOnly? toDate, string shopType);
        Task<List<PurchaseRequest>> GetPurchaseRequestsAsync(string status, string priority);
        Task<List<Fridge>> GetInventoryReportAsync();
    }

    public class CustomerManagementReportService : ICustomerManagementReportService
    {
        private readonly FridgeDbContext _context;

        public CustomerManagementReportService(FridgeDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, object>> GetCustomerManagementDashboardAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var firstDayOfMonth = new DateOnly(today.Year, today.Month, 1);

            var dashboard = new Dictionary<string, object>();

            // Customer Statistics - using your Customer class
            dashboard["TotalCustomers"] = await _context.Customers.CountAsync();
            dashboard["ActiveCustomers"] = await _context.Customers.CountAsync(c => c.IsActive);
            dashboard["NewCustomersThisMonth"] = await _context.Customers
                .CountAsync(c => c.RegistrationDate >= firstDayOfMonth);

            // Fridge Statistics - using your Fridge class
            dashboard["TotalFridges"] = await _context.Fridge.CountAsync(f => f.IsActive);
            dashboard["AllocatedFridges"] = await _context.Fridge.CountAsync(f => f.Status == "Allocated" && f.IsActive);
            dashboard["AvailableFridges"] = await _context.Fridge.CountAsync(f => f.Status == "Available" && f.IsActive);
            dashboard["FridgesUnderMaintenance"] = await _context.Fridge.CountAsync(f => f.Condition == "Under Repair" && f.IsActive);

            // Allocation Statistics - using your FridgeAllocation class
            dashboard["TotalAllocations"] = await _context.FridgeAllocation.CountAsync();
            dashboard["ActiveAllocations"] = await _context.FridgeAllocation.CountAsync(a => a.Status == "Active");
            dashboard["PendingReturns"] = await _context.FridgeAllocation.CountAsync(a => a.Status == "Pending Return");

            // Purchase Requests - using your PurchaseRequest class
            dashboard["PendingPurchaseRequests"] = await _context.PurchaseRequests.CountAsync(pr => pr.Status == "Pending");
            dashboard["UrgentPurchaseRequests"] = await _context.PurchaseRequests.CountAsync(pr => pr.Priority == "Urgent");

            // Inventory - using your Inventory class
            dashboard["LowStockItems"] = await _context.Inventory.CountAsync(i => i.Quantity < 5);

            // Recent Data using your actual entity classes
            dashboard["RecentCustomers"] = await _context.Customers
                .Include(c => c.Location)
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.RegistrationDate)
                .Take(6)
                .ToListAsync();

            dashboard["RecentAllocations"] = await _context.FridgeAllocation
                .Include(a => a.Customer)
                .Include(a => a.Fridge)
                .Where(a => a.Status == "Active")
                .OrderByDescending(a => a.AllocationDate)
                .Take(6)
                .ToListAsync();

            dashboard["UrgentPurchaseRequests"] = await _context.PurchaseRequests
                .Include(pr => pr.Fridge)
                .Where(pr => pr.Priority == "Urgent" && pr.Status == "Pending")
                .OrderBy(pr => pr.RequestDate)
                .Take(5)
                .ToListAsync();

            // Customer Type Distribution using your ShopType enum
            dashboard["CustomerTypeDistribution"] = await _context.Customers
                .Where(c => c.IsActive)
                .GroupBy(c => c.ShopType)
                .ToDictionaryAsync(g => g.Key.ToString(), g => g.Count());

            return dashboard;
        }

        public async Task<List<Customer>> GetCustomerReportAsync()
        {
            return await _context.Customers
                .Include(c => c.Location)
                .Include(c => c.FridgeAllocation)
                    .ThenInclude(a => a.Fridge)
                .Where(c => c.IsActive)
                .OrderBy(c => c.FullName)
                .ToListAsync();
        }

        public async Task<List<FridgeAllocation>> GetAllocationReportAsync(DateOnly? fromDate, DateOnly? toDate, string shopType)
        {
            fromDate ??= DateOnly.FromDateTime(DateTime.Today.AddMonths(-6));
            toDate ??= DateOnly.FromDateTime(DateTime.Today);

            var query = _context.FridgeAllocation
                .Include(a => a.Customer)
                    .ThenInclude(c => c.Location)
                .Include(a => a.Fridge)
                .Where(a => a.AllocationDate >= fromDate && a.AllocationDate <= toDate)
                .AsQueryable();

            // Apply shop type filter if specified
            if (!string.IsNullOrEmpty(shopType) && shopType != "All")
            {
                if (Enum.TryParse<ShopType>(shopType, out var shopTypeEnum))
                {
                    query = query.Where(a => a.Customer.ShopType == shopTypeEnum);
                }
            }

            return await query
                .OrderByDescending(a => a.AllocationDate)
                .ToListAsync();
        }

        public async Task<List<PurchaseRequest>> GetPurchaseRequestsAsync(string status, string priority)
        {
            var query = _context.PurchaseRequests
                .Include(pr => pr.Fridge)
                .Include(pr => pr.Inventory)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(pr => pr.Status == status);
            }

            if (!string.IsNullOrEmpty(priority) && priority != "All")
            {
                query = query.Where(pr => pr.Priority == priority);
            }

            return await query
                .OrderByDescending(pr => pr.RequestDate)
                .ToListAsync();
        }

        public async Task<List<Fridge>> GetInventoryReportAsync()
        {
            return await _context.Fridge
                .Include(f => f.Inventories)
                .Include(f => f.Supplier)
                .Include(f => f.Customer)
                .Where(f => f.IsActive)
                .OrderBy(f => f.FridgeType)
                .ThenBy(f => f.Model)
                .ToListAsync();
        }
    }
}