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
    public class CustomerManagementHomeController : Controller
    {
        private readonly FridgeDbContext _context;
        private readonly CustomerService _customerService;
        private readonly IMaintenanceRequestService _mrService;
        private readonly ILogger<CustomerManagementHomeController> _logger;
        private const int LowStockThreshold = 5;

        public CustomerManagementHomeController(
            FridgeDbContext context,
            CustomerService customerService,
            IMaintenanceRequestService mrService,
            ILogger<CustomerManagementHomeController> logger)
        {
            _context = context;
            _customerService = customerService;
            _mrService = mrService;
            _logger = logger;
        }

        // =========================== Dashboard/Home ===========================
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

        // =========================== Low Stock Alert ===========================
        private async Task<List<Fridge>> GetLowStockFridges()
        {
            return await _context.Fridge
                .Where(f => f.Quantity <= LowStockThreshold && f.IsActive)
                .ToListAsync();
        }

        // =========================== New Customers ===========================
        private async Task<List<Customer>> GetNewCustomers()
        {
            var last30Days = DateTime.Now.AddDays(-30);

            // Fetch all active customers first
            var customers = await _context.Customers
                .Where(c => c.IsActive)
                .ToListAsync();

            // Now filter using DateOnly in memory
            return customers
                .Where(c => c.RegistrationDate >= DateOnly.FromDateTime(last30Days))
                .ToList();
        }


        // =========================== Pending Allocations ===========================
        private async Task<List<PendingAllocationViewModel>> GetPendingAllocations()
        {
            var allocations = await _context.FridgeAllocation
                .Include(fa => fa.Customer)
                .Include(fa => fa.Fridge)
                .Include(fa => fa.OrderItem)
                .Where(fa => fa.ReturnDate == null || fa.ReturnDate > DateOnly.FromDateTime(DateTime.Today))
                .ToListAsync();

            return allocations.Select(fa => new PendingAllocationViewModel
            {
                CustomerId = fa.CustomerID,
                CustomerName = fa.Customer?.FullName ?? "N/A",
                OrderId = fa.OrderItem?.OrderId ?? 0,
                OrderItemId = fa.OrderItemId,
                FridgeId = fa.FridgeId,
                FridgeBrand = fa.Fridge?.Brand ?? "N/A",
                FridgeModel = fa.Fridge?.Model ?? "N/A",
                QuantityAllocated = fa.QuantityAllocated,
                QuantityOrdered = fa.OrderItem?.Quantity ?? 0,
                QuantityPending = (fa.OrderItem?.Quantity ?? 0) - fa.QuantityAllocated,
                Status = ((fa.OrderItem?.Quantity ?? 0) - fa.QuantityAllocated) > 0 ? "Pending" : "Allocated"
            }).ToList();
        }

        // =========================== Fridge Summary ===========================
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

        // =========================== Customers grouped by location ===========================
        private async Task<Dictionary<string, List<Customer>>> GetCustomersByLocation()
        {
            var customers = await _context.Customers.ToListAsync();
            var locations = await _context.Locations.ToListAsync();

            var result = new Dictionary<string, List<Customer>>();

            foreach (var location in locations)
            {
                result.Add(location.Address, customers.Where(c => c.LocationId == location.LocationId).ToList());
            }

            return result;
        }

        // =========================== Pending Purchase Requests ===========================
        private async Task<List<PurchaseRequest>> GetPendingPurchaseRequests()
        {
            return await _context.PurchaseRequests
                .Include(r => r.Fridge)
                .Where(r => r.Status == "Pending")
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
        }

        // =========================== Pending Maintenance Requests ===========================
        private async Task<List<MaintenanceRequest>> GetPendingMaintenanceRequests()
        {
            return await _context.MaintenanceRequest
                .Where(mr => mr.TaskStatus == Models.TaskStatus.Pending && mr.IsActive)
                .Include(mr => mr.Fridge)
                .ToListAsync();
        }
    }
}
