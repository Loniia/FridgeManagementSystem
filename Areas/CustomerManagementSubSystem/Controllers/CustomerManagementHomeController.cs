using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using FridgeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Areas.CustomerManagementSubSystem.Controllers
{
    [Area("CustomerManagementSubSystem")]
    public class CustomerManagemntHomeController : Controller
    {
        private readonly FridgeDbContext _context;
        private readonly CustomerService _customerService;
        private readonly IMaintenanceRequestService _mrService;
        private readonly ILogger<CustomerManagemntHomeController> _logger;
        private const int LowStockThreshold = 5;

        public CustomerManagemntHomeController(
            FridgeDbContext context,
            CustomerService customerService,
            IMaintenanceRequestService mrService,
            ILogger<CustomerManagemntHomeController> logger)
        {
            _context = context;
            _customerService = customerService;
            _mrService = mrService;
            _logger = logger;
        }

        // ===========================
        // Dashboard/Home
        // ===========================
        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                LowStockFridges = await GetLowStockFridges(),
                NewCustomers = await GetNewCustomers(),
                PendingAllocations = await GetPendingAllocations(),
                FridgeSummary = await GetFridgeSummary(),
                CustomersByLocation = await GetCustomersByLocation(),
                PendingPurchaseRequests = await GetPendingPurchaseRequests(),
                PendingMaintenanceRequests = await GetPendingMaintenanceRequests()
            };

            return View(model);
        }

        // ===========================
        // Low Stock Alert
        // ===========================
        private async Task<List<Fridge>> GetLowStockFridges()
        {
            return await _context.Fridge
                .Where(f => f.Quantity <= LowStockThreshold && f.IsActive)
                .ToListAsync();
        }

        // ===========================
        // New Customers
        // ===========================
        private async Task<List<Customer>> GetNewCustomers()
        {
            var last30Days = DateTime.Now.AddDays(-30);
            return await _context.Customers
                .Where(c => c.RegistrationDate.ToDateTime(TimeOnly.MinValue) >= last30Days)
                .ToListAsync();
        }

        // ===========================
        // Pending Allocations
        // ===========================
        private async Task<List<FridgeAllocation>> GetPendingAllocations()
        {
            return await _context.FridgeAllocation
                .Include(fa => fa.Customer)
                .Include(fa => fa.Fridge)
                .Where(fa => fa.ReturnDate == null || fa.ReturnDate > DateOnly.FromDateTime(DateTime.Today))
                .ToListAsync();
        }

        // ===========================
        // Fridge Summary
        // ===========================
        private async Task<FridgeSummaryViewModel> GetFridgeSummary()
        {
            var fridges = await _context.Fridge.ToListAsync();

            return new FridgeSummaryViewModel
            {
                TotalFridges = fridges.Count,
                Available = fridges.Count(f => f.Status == "Available"),
                Allocated = fridges.Count(f => f.Status == "Allocated"),
                Damaged = fridges.Count(f => f.Status == "Damaged"),
                Scrapped = fridges.Count(f => f.Status == "Scrapped")
            };
        }

        // ===========================
        // Customers grouped by location
        // ===========================
        private async Task<Dictionary<string, IEnumerable<Customer>>> GetCustomersByLocation()
        {
            var customers = await _context.Customers.ToListAsync();
            var locations = await _context.Locations.ToListAsync();

            var result = new Dictionary<string, IEnumerable<Customer>>();

            foreach (var location in locations)
            {
                var list = customers.Where(c => c.LocationId == location.LocationId).ToList();
                // Use City + Province as key since Location has no Name
                result.Add($"{location.City}, {location.Province}", list);
            }

            return result;
        }

        // ===========================
        // Pending Purchase Requests
        // ===========================
        private async Task<List<PurchaseRequest>> GetPendingPurchaseRequests()
        {
            return await _context.PurchaseRequests
                .Include(r => r.Fridge)
                .Where(r => r.Status == "Pending")
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
        }

        // ===========================
        // Pending Maintenance Requests
        // ===========================
        private async Task<List<MaintenanceRequest>> GetPendingMaintenanceRequests()
        {
            return await _context.MaintenanceRequest
                .Where(mr => mr.TaskStatus == Models.TaskStatus.Pending && mr.IsActive)
                .Include(mr => mr.Fridge)
                .ToListAsync();
        }
    }
}
