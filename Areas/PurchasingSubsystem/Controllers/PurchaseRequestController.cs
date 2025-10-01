using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;

using PurchasingSubsystem.Models;
using FridgeManagementSystem.Models;
#nullable disable
namespace PurchasingSubsystem.Controllers
{
    public class PurchaseRequestsController : Controller
    {
        private readonly FridgeDbContext _context;

        public PurchaseRequestsController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseRequests
        public async Task<IActionResult> Index()
        {
            return View(await _context.PurchaseRequests.ToListAsync());
        }

        // GET: PurchaseRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseRequest = await _context.PurchaseRequests
                .FirstOrDefaultAsync(m => m.PurchaseRequestID == id);
            if (purchaseRequest == null)
            {
                return NotFound();
            }

            return View(purchaseRequest);
        }

        // GET: PurchaseRequests/Create
        public IActionResult Create()
        {
            ViewBag.InventoryLiaisons = new SelectList(_context.InventoryLiaisons, "InventoryLiaisonID", "Name");
            return View();
        }

        // POST: PurchaseRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseRequestID,InventoryLiaisonId,RequestDate,Status")] PurchaseRequest purchaseRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseRequest);
        }

        // GET: PurchaseRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseRequest = await _context.PurchaseRequests.FindAsync(id);
            if (purchaseRequest == null)
            {
                return NotFound();
            }
            ViewBag.InventoryLiaisons = new SelectList(_context.InventoryLiaisons, "InventoryLiaisonID", "Name");
            return View(purchaseRequest);
        }

        // POST: PurchaseRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseRequestID,InventoryLiaisonId,RequestDate,Status")] PurchaseRequest purchaseRequest)
        {
            if (id != purchaseRequest.PurchaseRequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseRequestExists(purchaseRequest.PurchaseRequestID))
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
            return View(purchaseRequest);
        }

        // GET: PurchaseRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseRequest = await _context.PurchaseRequests
                .FirstOrDefaultAsync(m => m.PurchaseRequestID == id);
            if (purchaseRequest == null)
            {
                return NotFound();
            }

            return View(purchaseRequest);
        }

        // POST: PurchaseRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseRequest = await _context.PurchaseRequests.FindAsync(id);
            _context.PurchaseRequests.Remove(purchaseRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseRequestExists(int id)
        {
            return _context.PurchaseRequests.Any(e => e.PurchaseRequestID == id);
        }
    }
}
