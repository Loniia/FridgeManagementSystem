using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchasingSubsystem.Data;
using PurchasingSubsystem.Models;

namespace PurchasingSubsystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PurchasingSubsystem.Data;
    using PurchasingSubsystem.Models;

    namespace PurchasingSubsystem.Areas.Purchasing.Controllers
    {
        [Area("Purchasing")] // ADD THIS for area routing
        public class SuppliersController : Controller
        {
            private readonly ApplicationDbContext _context;

            public SuppliersController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: Purchasing/Suppliers
            public async Task<IActionResult> Index()
            {
                var activeSuppliers = await _context.Suppliers.Where(s => s.IsActive).ToListAsync();
                return View(activeSuppliers);
            }

            // GET: Purchasing/Suppliers/Details/5
            public async Task<IActionResult> Details(int? id)
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

            // GET: Purchasing/Suppliers/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Purchasing/Suppliers/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            // FIXED: Added ContactPerson and corrected property names
            public async Task<IActionResult> Create([Bind("SupplierID,Name,ContactPerson,Email,Phone,Address")] Supplier supplier)
            {
                if (ModelState.IsValid)
                {
                    // Set default values
                    supplier.IsActive = true;

                    _context.Add(supplier);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = $"Supplier '{supplier.Name}' was created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                return View(supplier);
            }

            // GET: Purchasing/Suppliers/Edit/5
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

            // POST: Purchasing/Suppliers/Edit/5
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
                        TempData["Message"] = $"Changes to '{supplier.Name}' were saved successfully.";
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

            // GET: Purchasing/Suppliers/Delete/5
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

            // POST: Purchasing/Suppliers/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var supplier = await _context.Suppliers.FindAsync(id);
                if (supplier != null)
                {
                    supplier.IsActive = false;
                    _context.Suppliers.Update(supplier);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = $"Supplier '{supplier.Name}' was deleted successfully.";
                }
                return RedirectToAction(nameof(Index));
            }

            private bool SupplierExists(int id)
            {
                return _context.Suppliers.Any(e => e.SupplierID == id);
            }
        }
    }
}
