using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class ManageSupplierController : Controller
    {
        private readonly FridgeDbContext _context;

        public ManageSupplierController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: Administrator/ManageSupplier
        public async Task<IActionResult> Index()
        {
            var suppliers = await _context.Suppliers
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
            return View(suppliers);
        }

        // GET: Administrator/ManageSupplier/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id && m.IsActive);

            if (supplier == null) return NotFound();
            return View(supplier);
        }

        // GET: Administrator/ManageSupplier/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrator/ManageSupplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.IsActive = true;
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // Other actions (Edit, Delete) would go here...
        // GET: Administrator/ManageSupplier/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return NotFound();

            return View(supplier);
        }

        // POST: Administrator/ManageSupplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier supplier)
        {
            if (id != supplier.SupplierID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Supplier updated successfully!";
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

        // GET: Administrator/ManageSupplier/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id);

            if (supplier == null) return NotFound();
            return View(supplier);
        }

        // POST: Administrator/ManageSupplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                // Soft delete by setting IsActive to false
                supplier.IsActive = false;
                _context.Update(supplier);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Supplier deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.SupplierID == id);
        }
    }
}
