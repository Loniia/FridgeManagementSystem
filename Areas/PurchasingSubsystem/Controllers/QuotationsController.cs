using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Areas.PurchasingSubsystem.Controllers
{
    [Area("PurchasingSubsystem")]
    public class QuotationsController : Controller
    {
        private readonly FridgeDbContext _context;

        public QuotationsController(FridgeDbContext context)
        {
            _context = context;
        }

        private string GeneratePONumber()
        {
            var count = _context.PurchaseOrders.Count(po => po.OrderDate.Year == DateTime.Now.Year) + 1;
            return $"PO-{DateTime.Now:yyyy}-{count:D3}";
        }

        // GET: PurchasingSubsystem/Quotation
        public async Task<IActionResult> Index()
        {
            var quotations = await _context.Quotations
                .Include(q => q.Supplier)
                .Include(q => q.RequestForQuotation)
                .OrderByDescending(q => q.ReceivedDate)
                .ToListAsync();

            return View(quotations);
        }

        // GET: PurchasingSubsystem/Quotations/Create
        public async Task<IActionResult> Create()
        {
            // Load only active or open RFQs
            ViewBag.RFQs = await _context.RequestsForQuotation
                .Where(r => r.Status == "Sent" || r.Status == "Open")
                .Select(r => new SelectListItem
                {
                    Value = r.RFQID.ToString(),
                    Text = $"{r.RFQNumber} - {r.Description}"
                })
                .ToListAsync();

            // Load active suppliers
            ViewBag.Suppliers = await _context.Suppliers
                .Where(s => s.IsActive)
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierID.ToString(),
                    Text = $"{s.Name} ({s.Email})"
                })
                .ToListAsync();

            return View();
        }


        // POST: PurchasingSubsystem/Quotations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quotation quotation)
        {
            if (ModelState.IsValid)
            {
                quotation.ReceivedDate = DateTime.Now;
                quotation.Status = "Pending";

                _context.Quotations.Add(quotation);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Quotation created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdowns in case of validation error
            ViewBag.RFQs = await _context.RequestsForQuotation
                .Where(r => r.Status == "Sent" || r.Status == "Open")
                .Select(r => new SelectListItem
                {
                    Value = r.RFQID.ToString(),
                    Text = $"{r.RFQNumber} - {r.Description}"
                })
                .ToListAsync();

            ViewBag.Suppliers = await _context.Suppliers
                .Where(s => s.IsActive)
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierID.ToString(),
                    Text = $"{s.Name} ({s.Email})"
                })
                .ToListAsync();

            return View(quotation);
        }


        // GET: PurchasingSubsystem/Quotation/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var quotation = await _context.Quotations
                .Include(q => q.Supplier)
                .Include(q => q.RequestForQuotation)
                .FirstOrDefaultAsync(q => q.QuotationID == id);

            if (quotation == null)
                return NotFound();

            return View(quotation);
        }

        // POST: PurchasingSubsystem/Quotation/Approve/5
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var quotation = await _context.Quotations
                .Include(q => q.Supplier)
                .Include(q => q.RequestForQuotation)
                .FirstOrDefaultAsync(q => q.QuotationID == id);

            if (quotation == null)
                return NotFound();

            quotation.Status = "Approved";
            await _context.SaveChangesAsync();

            // Automatically generate Purchase Order - FIXED: Remove DeliveryTimeframe reference
            var purchaseOrder = new PurchaseOrder
            {
                PONumber = GeneratePONumber(),
                QuotationID = quotation.QuotationID,
                SupplierID = quotation.SupplierId,
                TotalAmount = quotation.QuotationAmount,
                OrderDate = DateTime.Now,
                Status = "Ordered",
                ExpectedDeliveryDate = DateTime.Now.AddDays(14), // Use fixed value instead of DeliveryTimeframe
                DeliveryAddress = "Main Warehouse - Johannesburg",
                SpecialInstructions = $"Generated from quotation #{quotation.QuotationID}",
                IsActive = true
            };

            _context.PurchaseOrders.Add(purchaseOrder);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Quotation approved and Purchase Order {purchaseOrder.PONumber} created!";
            return RedirectToAction(nameof(Index));
        }

        // POST: PurchasingSubsystem/Quotation/Reject/5
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var quotation = await _context.Quotations.FindAsync(id);
            if (quotation == null)
                return NotFound();

            quotation.Status = "Rejected";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Quotation rejected successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool QuotationExists(int id)
        {
            return _context.Quotations.Any(q => q.QuotationID == id);
        }
    }
}

