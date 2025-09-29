using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PurchasingSubsystem.Data;
using PurchasingSubsystem.Models;
#nullable disable
namespace PurchasingSubsystem.Controllers
{
    public class QuotationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuotationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Quotations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Quotations.ToListAsync());
        }

        // GET: Quotations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations
                .FirstOrDefaultAsync(m => m.QuotationID == id);
            if (quotation == null)
            {
                return NotFound();
            }

            return View(quotation);
        }

        // GET: Quotations/Create
        public IActionResult Create()
        {
            ViewBag.Suppliers = new SelectList(_context.Suppliers, "SupplierID", "Name");
            ViewBag.RequestForQuotations = new SelectList(_context.RequestsForQuotation, "RFQID", "RFQID");
            return View();
        }

        // POST: Quotations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuotationID,RequestForQuotationId,SupplierId,ReceivedDate,QuotationAmount")] Quotation quotation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quotation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quotation);
        }

        // GET: Quotations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations.FindAsync(id);
            if (quotation == null)
            {

                return NotFound();
            }
            ViewBag.Suppliers = new SelectList(_context.Suppliers, "SupplierID", "Name");
            ViewBag.RequestForQuotations = new SelectList(_context.RequestsForQuotation, "RFQID", "RFQID");
            return View(quotation);
        }

        // POST: Quotations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuotationID,RequestForQuotationId,SupplierId,ReceivedDate,QuotationAmount")] Quotation quotation)
        {
            if (id != quotation.QuotationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quotation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuotationExists(quotation.QuotationID))
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
            return View(quotation);
        }

        // GET: Quotations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations
                .FirstOrDefaultAsync(m => m.QuotationID == id);
            if (quotation == null)
            {
                return NotFound();
            }

            return View(quotation);
        }

        // POST: Quotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quotation = await _context.Quotations.FindAsync(id);
            _context.Quotations.Remove(quotation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuotationExists(int id)
        {
            return _context.Quotations.Any(e => e.QuotationID == id);
        }
    }
}
