using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchasingSubsystem.Data;
using PurchasingSubsystem.Models;

namespace PurchasingSubsystem.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            // FIXED: Only get ACTIVE suppliers (soft delete)
            var activeSuppliers = await _context.Suppliers.Where(s => s.IsActive).ToListAsync();
            return View(activeSuppliers); // You can remove the full path, just return View(...) is enough
        }
        // GET: Suppliers
        ////public async Task<IActionResult> Index()
        ////{
        ////    return View("~/Views/Suppliers/Index.cshtml", await _context.Suppliers.ToListAsync());
        ////}

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var supplier = await _context.Suppliers
            //    .FirstOrDefaultAsync(m => m.SupplierID == id);
            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id && m.IsActive); // Added IsActive check for detail
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierID,Name,Email,Phone,Address")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                TempData["Message"] = $"Supplier '{supplier.Name}' was created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierID,Name,Email,Phone,Address,IsActive")] Supplier supplier)
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
                TempData["Message"] = $"Changes to '{supplier.Name}' were saved successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
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

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                supplier.IsActive = false;   // soft delete instead of removing
                _context.Suppliers.Update(supplier);
                await _context.SaveChangesAsync();
                TempData["Message"] = $"Supplier '{supplier.Name}' was deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }


        private bool SupplierExists(int id)
        {
            // Check if any supplier exists with this ID, regardless of IsActive status
            return _context.Suppliers.Any(e => e.SupplierID == id);
        }
    }
}
