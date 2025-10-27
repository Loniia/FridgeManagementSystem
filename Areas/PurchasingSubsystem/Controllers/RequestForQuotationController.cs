using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Areas.PurchasingSubsystem.Controllers
{
    [Area("PurchasingSubsystem")]
    public class RequestForQuotationController : Controller
    {
        private readonly FridgeDbContext _context;

        public RequestForQuotationController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: PurchasingSubsystem/RequestForQuotation
        public async Task<IActionResult> Index()
        {
            var rfqs = await _context.RequestsForQuotation
                .Include(r => r.PurchaseRequest)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            return View(rfqs);
        }

        // GET: PurchasingSubsystem/RequestForQuotation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var rfq = await _context.RequestsForQuotation
                .Include(r => r.PurchaseRequest)
                .FirstOrDefaultAsync(r => r.RFQID == id);

            if (rfq == null)
                return NotFound();

            return View(rfq);
        }

        // (Optional) GET: PurchasingSubsystem/RequestForQuotation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var rfq = await _context.RequestsForQuotation
                .Include(r => r.PurchaseRequest)
                .FirstOrDefaultAsync(r => r.RFQID == id);

            if (rfq == null)
                return NotFound();

            return View(rfq);
        }

        // (Optional) POST: Delete Confirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rfq = await _context.RequestsForQuotation.FindAsync(id);
            if (rfq != null)
            {
                _context.RequestsForQuotation.Remove(rfq);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "RFQ deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}

