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
    public class AdminFridgeController : Controller
    {
        private readonly FridgeService _fridgeService;
        private readonly FridgeDbContext _context;

        public AdminFridgeController(FridgeService fridgeService, FridgeDbContext context)
        {
            _fridgeService = fridgeService;
            _context = context;
        }

        // ===== View all fridges (Inventory + Status + Supplier + Location + Maintenance Info) =====
        public async Task<IActionResult> Index()
        {
            var fridges = await _fridgeService.GetAllFridgesWithMaintenanceAsync();
            return View(fridges);
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

