using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = Roles.Admin)]
    public class ManageFridgeController : Controller
    {
        private readonly FridgeService _fridgeService;
        private readonly FridgeDbContext _context;

        public ManageFridgeController(FridgeService fridgeService, FridgeDbContext context)
        {
            _fridgeService = fridgeService;
            _context = context;
        }

        // ===== View all fridges (Inventory + Status + Supplier + Location + Maintenance Info) =====
        public async Task<IActionResult> Index()
        {
            // Get all fridges including related entities
            var fridges = await _context.Fridge
                .Include(f => f.Supplier)
                .Include(f => f.Customer)
                .Include(f => f.Location)
                .Include(f => f.Faults)
                .Include(f => f.MaintenanceRequest)
                .ToListAsync();

            // Map to a ViewModel for display
            var fridgeVMs = fridges.Select(f => new FridgeViewModel
            {
                FridgeId = f.FridgeId,
               
                FridgeType = f.FridgeType,
                Brand = f.Brand,
                Model = f.Model,
                SerialNumber = f.SerialNumber,
                Condition = f.Condition,
                PurchaseDate = f.PurchaseDate,
                WarrantyExpiry = f.WarrantyExpiry,
                
                UpdatedDate = f.UpdatedDate,
                DateAdded = f.DateAdded,
                SupplierID = f.SupplierID,
                CustomerID = f.CustomerID,
                LocationId = f.LocationId,
                Quantity = f.Quantity,
                DeliveryDate = f.DeliveryDate,
                Status = f.Status,
                IsActive = f.IsActive,

                // Related info
                CustomerName = f.Customer?.FullName ?? "N/A",
                AllocationDate = f.FridgeAllocation?.FirstOrDefault()?.AllocationDate,
                ReturnDate = f.FridgeAllocation?.FirstOrDefault()?.ReturnDate,

            }).ToList();

            return View(fridgeVMs);
        }

        // ===== Create new fridge =====
        public IActionResult Create()
        {
            ViewBag.Suppliers = new SelectList(_context.Suppliers, "SupplierId", "Name");
            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "City");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fridge fridge)
        {
            if (ModelState.IsValid)
            {
                await _fridgeService.CreateFridgeAsync(fridge);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Suppliers = new SelectList(_context.Suppliers, "SupplierId", "Name", fridge.SupplierID);
            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "City", fridge.LocationId);
            return View(fridge);
        }

        // ===== Edit fridge details =====
        public async Task<IActionResult> Edit(int id)
        {
            var fridge = await _fridgeService.GetFridgeByIdAsync(id);
            if (fridge == null) return NotFound();

            ViewBag.Suppliers = new SelectList(_context.Suppliers, "SupplierId", "Name", fridge.SupplierID);
            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "City", fridge.LocationId);
            return View(fridge);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Fridge fridge)
        {
            if (id != fridge.FridgeId) return NotFound();

            if (ModelState.IsValid)
            {
                await _fridgeService.UpdateFridgeAsync(fridge);
                return RedirectToAction(nameof(Index));
            }
            return View(fridge);
        }

        // ===== Delete fridge =====
        public async Task<IActionResult> Delete(int id)
        {
            await _fridgeService.DeleteFridgeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // ===== Inventory Reporting =====
        public async Task<IActionResult> InventoryReport()
        {
            // This report includes stock levels per location + fridge status + last maintenance
            var report = await _fridgeService.GetInventoryReportAsync();
            return View(report);
        }

        // ===== Supplier Reporting =====
        public async Task<IActionResult> SupplierReport()
        {
            var report = await _fridgeService.GetSupplierReportAsync();
            return View(report);
        }
    }
}

