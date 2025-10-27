using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Areas.PurchasingSubsystem.Controllers
{
    [Area("PurchasingSubsystem")]
    public class DeliveryNotesController : Controller
    {
        private readonly FridgeDbContext _context;

        public DeliveryNotesController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: PurchasingSubsystem/DeliveryNotes
        public async Task<IActionResult> Index()
        {
            var deliveryNotes = await _context.DeliveryNotes
                .Include(dn => dn.purchaseOrder)
                .ThenInclude(po => po.Supplier)
                .OrderByDescending(dn => dn.DeliveryDate)
                .ToListAsync();

            return View(deliveryNotes);
        }

        // GET: PurchasingSubsystem/DeliveryNotes/Create
        public async Task<IActionResult> Create(int? purchaseOrderId)
        {
            // If purchaseOrderId is provided, pre-select that PO
            var purchaseOrders = await _context.PurchaseOrders
                .Where(po => po.IsActive && (po.Status == "Ordered" || po.Status == "Shipped"))
                .Include(po => po.Supplier)
                .ToListAsync();

            ViewBag.PurchaseOrders = new SelectList(
                purchaseOrders,
                "PurchaseOrderID",
                "PONumber",
                purchaseOrderId
            );

            return View();
        }

        // POST: PurchasingSubsystem/DeliveryNotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeliveryNote deliveryNote)
        {
            if (ModelState.IsValid)
            {
                deliveryNote.DeliveryNumber = GenerateDeliveryNoteNumber();
                deliveryNote.DeliveryDate = DateTime.Today;
                deliveryNote.IsActive = true;

                // Auto-set supplier from purchase order
                var purchaseOrder = await _context.PurchaseOrders
                    .Include(po => po.Supplier)
                    .FirstOrDefaultAsync(po => po.PurchaseOrderID == deliveryNote.PurchaseOrderId);

                if (purchaseOrder != null)
                {
                    deliveryNote.SupplierId = purchaseOrder.SupplierID;
                }

                _context.DeliveryNotes.Add(deliveryNote);

                // Update Purchase Order status to Delivered
                if (purchaseOrder != null)
                {
                    purchaseOrder.Status = "Delivered";
                    _context.PurchaseOrders.Update(purchaseOrder);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Delivery Note {deliveryNote.DeliveryNumber} created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown if validation fails
            var purchaseOrders = await _context.PurchaseOrders
                .Where(po => po.IsActive && (po.Status == "Ordered" || po.Status == "Shipped"))
                .Include(po => po.Supplier)
                .ToListAsync();

            ViewBag.PurchaseOrders = new SelectList(
                purchaseOrders,
                "PurchaseOrderID",
                "PONumber",
                deliveryNote.PurchaseOrderId
            );

            return View(deliveryNote);
        }

        // GET: PurchasingSubsystem/DeliveryNotes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var deliveryNote = await _context.DeliveryNotes
                .Include(dn => dn.purchaseOrder)
                .ThenInclude(po => po.Supplier)
                .Include(dn => dn.Supplier)
                .FirstOrDefaultAsync(dn => dn.DeliveryNoteID == id);

            if (deliveryNote == null)
                return NotFound();

            return View(deliveryNote);
        }

        // GET: PurchasingSubsystem/DeliveryNotes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var deliveryNote = await _context.DeliveryNotes
                .Include(dn => dn.purchaseOrder)
                .FirstOrDefaultAsync(dn => dn.DeliveryNoteID == id);

            if (deliveryNote == null)
                return NotFound();

            return View(deliveryNote);
        }

        // POST: PurchasingSubsystem/DeliveryNotes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DeliveryNote deliveryNote)
        {
            if (id != deliveryNote.DeliveryNoteID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryNote);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Delivery note updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryNoteExists(id))
                        return NotFound();
                    throw;
                }
            }
            return View(deliveryNote);
        }

        // POST: PurchasingSubsystem/DeliveryNotes/Verify/5
        [HttpPost]
        public async Task<IActionResult> Verify(int id)
        {
            var deliveryNote = await _context.DeliveryNotes.FindAsync(id);
            if (deliveryNote == null)
                return NotFound();

            deliveryNote.IsVerified = true;
            deliveryNote.VerificationDate = DateTime.Now;
            _context.DeliveryNotes.Update(deliveryNote);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Delivery note verified successfully!";
            return RedirectToAction(nameof(Index));
        }

        // POST: PurchasingSubsystem/DeliveryNotes/MarkAsReceived/5
        [HttpPost]
        public async Task<IActionResult> MarkAsReceived(int id)
        {
            var deliveryNote = await _context.DeliveryNotes.FindAsync(id);
            if (deliveryNote == null)
                return NotFound();

            deliveryNote.IsReceivedInInventory = true;
            deliveryNote.InventoryReceiptDate = DateTime.Now;
            _context.DeliveryNotes.Update(deliveryNote);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Delivery marked as received in inventory!";
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryNoteExists(int id)
        {
            return _context.DeliveryNotes.Any(e => e.DeliveryNoteID == id);
        }

        private string GenerateDeliveryNoteNumber()
        {
            var count = _context.DeliveryNotes.Count(dn => dn.DeliveryDate.Year == DateTime.Now.Year) + 1;
            return $"DN-{DateTime.Now:yyyy}-{count:D3}";
        }
    }
}