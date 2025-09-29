using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PurchasingSubsystem.Data;
using PurchasingSubsystem.Models;
#nullable disable
namespace PurchasingSubsystem.Controllers
{
    public class DeliveryNotesController : Controller
    {
        private readonly FridgeDbContext _context;

        public DeliveryNotesController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: DeliveryNotes
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeliveryNotes.ToListAsync());
        }

        // GET: DeliveryNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryNote = await _context.DeliveryNotes
                .FirstOrDefaultAsync(m => m.DeliveryNoteID == id);
            if (deliveryNote == null)
            {
                return NotFound();
            }

            return View(deliveryNote);
        }

        // GET: DeliveryNotes/Create
        public IActionResult Create()
        {
            ViewBag.Suppliers = new SelectList(_context.Suppliers, "SupplierID", "Name");
            ViewBag.PurchaseOrders = new SelectList(_context.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderID");
            return View();
        }

        // POST: DeliveryNotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeliveryNoteID,PurchaseOrderId,SupplierId,DeliveryDate")] DeliveryNote deliveryNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryNote);
        }

        // GET: DeliveryNotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryNote = await _context.DeliveryNotes.FindAsync(id);
            if (deliveryNote == null)
            {
                return NotFound();
            }
            ViewBag.Suppliers = new SelectList(_context.Suppliers, "SupplierID", "Name");
            ViewBag.PurchaseOrders = new SelectList(_context.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderID");
            return View(deliveryNote);
        }

        // POST: DeliveryNotes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeliveryNoteID,PurchaseOrderId,SupplierId,DeliveryDate")] DeliveryNote deliveryNote)
        {
            if (id != deliveryNote.DeliveryNoteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryNoteExists(deliveryNote.DeliveryNoteID))
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
            return View(deliveryNote);
        }

        // GET: DeliveryNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryNote = await _context.DeliveryNotes
                .FirstOrDefaultAsync(m => m.DeliveryNoteID == id);
            if (deliveryNote == null)
            {
                return NotFound();
            }

            return View(deliveryNote);
        }

        // POST: DeliveryNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deliveryNote = await _context.DeliveryNotes.FindAsync(id);
            _context.DeliveryNotes.Remove(deliveryNote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryNoteExists(int id)
        {
            return _context.DeliveryNotes.Any(e => e.DeliveryNoteID == id);
        }
    }
}
