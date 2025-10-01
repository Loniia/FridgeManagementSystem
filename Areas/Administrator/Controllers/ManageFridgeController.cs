using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class ManageFridgeController : Controller
    {
        private readonly FridgeService _fridgeService;
        private readonly FridgeDbContext _context; // for dropdowns only

        public ManageFridgeController(FridgeService fridgeService, FridgeDbContext context)
        {
            _fridgeService = fridgeService;
            _context = context;
        }

        // ===== CRUD =====
        public async Task<IActionResult> Index()
        {
            var fridges = await _fridgeService.GetAllFridgesAsync();
            return View(fridges);
        }

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
            return View(fridge);
        }

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

        public async Task<IActionResult> Delete(int id)
        {
            await _fridgeService.DeleteFridgeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AllocateToCustomer(int id)
        {
            var fridge = await _context.Fridge.FindAsync(id);
            if (fridge == null) return NotFound();

            var customers = await _context.Customers
                .Select(c => new SelectListItem { Value = c.CustomerID.ToString(), Text = c.FullName })
                .ToListAsync();

            var vm = new AllocateFridgeViewModel
            {
                FridgeId = fridge.FridgeId,
                FridgeSerialNumber = fridge.SerialNumber,
                Customers = customers
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> DeallocateFromCustomer(int customerId)
        {
            var success = await _fridgeService.DeallocateFromCustomerAsync(customerId);
            if (!success) return BadRequest("Deallocation failed.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ScheduleMaintenance(int id)
        {
            var fridge = await _context.Fridge.FindAsync(id);
            if (fridge == null) return NotFound();

            var vm = new ScheduleMaintenanceViewModel
            {
                FridgeId = fridge.FridgeId,
                FridgeSerialNumber = fridge.SerialNumber,
                ScheduledDate = DateTime.Today
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> CompleteMaintenance(int fridgeId, int scheduleId)
        {
            var success = await _fridgeService.CompleteMaintenanceAsync(fridgeId, scheduleId);
            if (!success) return BadRequest("Completion failed.");
            return RedirectToAction(nameof(Index));
        }

        // ===== REPORTING =====
        public async Task<IActionResult> InventoryReport()
        {
            var report = await _fridgeService.GetInventoryReportAsync();
            return View(report);
        }

        public async Task<IActionResult> SupplierReport()
        {
            var report = await _fridgeService.GetSupplierReportAsync();
            return View(report);
        }
    }
}
