using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchasingSubsystem.Models;
using FridgeManagementSystem.Data;

#nullable disable
namespace PurchasingSubsystem.Controllers
{
    public class PurchaseManagersController : Controller
    {
        private readonly FridgeDbContext _context;

        public PurchaseManagersController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseManagers
        public async Task<IActionResult> Index()
        {
            return View(await _context.PurchaseManagers.ToListAsync());
        }

        // GET: PurchaseManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseManager = await _context.PurchaseManagers
                .FirstOrDefaultAsync(m => m.PurchaseManagerID == id);
            if (purchaseManager == null)
            {
                return NotFound();
            }

            return View(purchaseManager);
        }

        // GET: PurchaseManagers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PurchaseManagers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseManagerID,PurchaseManagerName,PurchaseManagerEmail")] PurchaseManager purchaseManager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseManager);
        }

        // GET: PurchaseManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseManager = await _context.PurchaseManagers.FindAsync(id);
            if (purchaseManager == null)
            {
                return NotFound();
            }
            return View(purchaseManager);
        }

        // POST: PurchaseManagers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseManagerID,PurchaseManagerName,PurchaseManagerEmail")] PurchaseManager purchaseManager)
        {
            if (id != purchaseManager.PurchaseManagerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseManagerExists(purchaseManager.PurchaseManagerID))
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
            return View(purchaseManager);
        }

        // GET: PurchaseManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseManager = await _context.PurchaseManagers
                .FirstOrDefaultAsync(m => m.PurchaseManagerID == id);
            if (purchaseManager == null)
            {
                return NotFound();
            }

            return View(purchaseManager);
        }

        // POST: PurchaseManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseManager = await _context.PurchaseManagers.FindAsync(id);
            _context.PurchaseManagers.Remove(purchaseManager);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseManagerExists(int id)
        {
            return _context.PurchaseManagers.Any(e => e.PurchaseManagerID == id);
        }
    }
}
