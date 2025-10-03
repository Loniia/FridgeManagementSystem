using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administration")]
    public class SuppliersController : Controller
    {
        private readonly FridgeDbContext _context;

        public SuppliersController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: Administration/Suppliers
        public async Task<IActionResult> Index()
        {
            var suppliers = await _context.Suppliers
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
            return View(suppliers);
        }

        // GET: Administration/Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .Include(s => s.PurchaseOrders)
                .Include(s => s.Quotations)
                .FirstOrDefaultAsync(m => m.SupplierID == id && m.IsActive);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Administration/Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administration/Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ContactPerson,Email,Phone,Address")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.IsActive = true;
                _context.Add(supplier);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Supplier '{supplier.Name}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Administration/Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null || !supplier.IsActive)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Administration/Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierID,Name,ContactPerson,Email,Phone,Address,IsActive")] Supplier supplier)
        {
            if (id != supplier.SupplierID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Supplier '{supplier.Name}' updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.SupplierID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Administration/Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id && m.IsActive);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Administration/Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                // Soft delete - set IsActive to false instead of removing
                supplier.IsActive = false;
                _context.Suppliers.Update(supplier);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Supplier '{supplier.Name}' deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.SupplierID == id);
        }
    }
}
