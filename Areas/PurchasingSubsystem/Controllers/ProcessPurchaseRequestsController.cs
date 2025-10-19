using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Areas.PurchasingSubsystem.Controllers
{
    [Area("PurchasingSubsystem")]
    public class ProcessPurchaseRequestsController : Controller
    {
        private readonly FridgeDbContext _context;

        public ProcessPurchaseRequestsController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: PurchasingSubsystem/ProcessPurchaseRequests/Pending
        public async Task<IActionResult> Pending()
        {
            var pendingRequests = await _context.PurchaseRequests
                .Include(pr => pr.Fridge)
                .Include(pr => pr.Inventory)
                .Where(pr => pr.Status == "Pending" && pr.IsActive)
                .OrderByDescending(pr => pr.RequestDate)
                .ToListAsync();

            return View(pendingRequests);
        }

        // GET: PurchasingSubsystem/ProcessPurchaseRequests/Approved
        public async Task<IActionResult> Approved()
        {
            var approvedRequests = await _context.PurchaseRequests
                .Include(pr => pr.Fridge)
                .Include(pr => pr.Inventory)
                .Where(pr => pr.Status == "Approved" && pr.IsActive)
                .OrderByDescending(pr => pr.ApprovalDate)
                .ToListAsync();

            return View(approvedRequests);
        }

        // GET: PurchasingSubsystem/ProcessPurchaseRequests/Rejected
        public async Task<IActionResult> Rejected()
        {
            var rejectedRequests = await _context.PurchaseRequests
                .Include(pr => pr.Fridge)
                .Include(pr => pr.Inventory)
                .Where(pr => pr.Status == "Rejected" && pr.IsActive)
                .OrderByDescending(pr => pr.RequestDate)
                .ToListAsync();

            return View(rejectedRequests);
        }

        // GET: PurchasingSubsystem/ProcessPurchaseRequests/Process/5
        public async Task<IActionResult> Process(int? id)
        {
            if (id == null) return NotFound();

            var purchaseRequest = await _context.PurchaseRequests
                .Include(pr => pr.Fridge)
                .Include(pr => pr.Inventory)
                .FirstOrDefaultAsync(pr => pr.PurchaseRequestID == id);

            if (purchaseRequest == null) return NotFound();

            // Mark as viewed
            if (!purchaseRequest.IsViewed)
            {
                purchaseRequest.IsViewed = true;
                purchaseRequest.ViewedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            // FIXED: Correct supplier property names
            ViewBag.Suppliers = await _context.Suppliers
                .Where(s => s.IsActive)
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierID.ToString(),
                    Text = $"{s.Name} - {s.Email} - {s.Phone}" // ← FIXED!
                })
                .ToListAsync();

            return View(purchaseRequest);
        }

        // POST: PurchasingSubsystem/ProcessPurchaseRequests/Process/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(int id, string approvalNotes, int[] selectedSuppliers, string action)
        {
            var purchaseRequest = await _context.PurchaseRequests
                .FirstOrDefaultAsync(pr => pr.PurchaseRequestID == id);

            if (purchaseRequest == null) return NotFound();

            if (action == "approve")
            {
                // 1. Update Purchase Request
                purchaseRequest.Status = "Approved";
                purchaseRequest.ApprovalDate = DateTime.Now;
                purchaseRequest.ApprovalNotes = approvalNotes;

                // 2. Create RFQ for each selected supplier
                if (selectedSuppliers != null && selectedSuppliers.Length > 0)
                {
                    foreach (var supplierId in selectedSuppliers)
                    {
                        var rfq = new RequestForQuotation
                        {
                            RFQNumber = GenerateRFQNumber(),
                            PurchaseRequestID = purchaseRequest.PurchaseRequestID,
                            Description = $"RFQ for {purchaseRequest.Quantity} x {purchaseRequest.ItemFullNames}",
                            RequiredQuantity = purchaseRequest.Quantity,
                            CreatedDate = DateTime.Now,
                            Deadline = DateTime.Now.AddDays(7),
                            Status = "Sent"
                        };

                        _context.RequestsForQuotation.Add(rfq);
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Purchase request approved and {selectedSuppliers.Length} RFQ(s) created!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Please select at least one supplier.";
                    return RedirectToAction(nameof(Process), new { id });
                }
            }
            else if (action == "reject")
            {
                purchaseRequest.Status = "Rejected";
                purchaseRequest.ApprovalDate = DateTime.Now;
                purchaseRequest.ApprovalNotes = approvalNotes;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Purchase request rejected.";
            }

            return RedirectToAction(nameof(Pending));
        }

        // GET: PurchasingSubsystem/ProcessPurchaseRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var purchaseRequest = await _context.PurchaseRequests
                .Include(pr => pr.Fridge)
                .Include(pr => pr.Inventory)
                .FirstOrDefaultAsync(pr => pr.PurchaseRequestID == id);

            if (purchaseRequest == null) return NotFound();

            return View(purchaseRequest);
        }

        // GET: PurchasingSubsystem/ProcessPurchaseRequests/QuickApprove/5
        public async Task<IActionResult> QuickApprove(int? id)
        {
            if (id == null) return NotFound();

            var purchaseRequest = await _context.PurchaseRequests
                .FirstOrDefaultAsync(pr => pr.PurchaseRequestID == id);

            if (purchaseRequest == null) return NotFound();

            // Get the first active supplier for quick approval
            var defaultSupplier = await _context.Suppliers
                .Where(s => s.IsActive)
                .FirstOrDefaultAsync();

            if (defaultSupplier != null)
            {
                // Auto-approve and create RFQ
                purchaseRequest.Status = "Approved";
                purchaseRequest.ApprovalDate = DateTime.Now;
                purchaseRequest.ApprovalNotes = "Quick-approved by Purchasing Manager";

                var rfq = new RequestForQuotation
                {
                    RFQNumber = GenerateRFQNumber(),
                    PurchaseRequestID = purchaseRequest.PurchaseRequestID,
                    Description = $"RFQ for {purchaseRequest.Quantity} x {purchaseRequest.ItemFullNames}",
                    RequiredQuantity = purchaseRequest.Quantity,
                    CreatedDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(7),
                    Status = "Sent"
                };

                _context.RequestsForQuotation.Add(rfq);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Purchase request quick-approved and RFQ created!";
            }
            else
            {
                TempData["ErrorMessage"] = "No active suppliers found. Please add suppliers first.";
            }

            return RedirectToAction(nameof(Pending));
        }

        // GET: PurchasingSubsystem/ProcessPurchaseRequests/Stats
        public async Task<IActionResult> Stats()
        {
            var stats = new
            {
                TotalRequests = await _context.PurchaseRequests.CountAsync(pr => pr.IsActive),
                PendingRequests = await _context.PurchaseRequests.CountAsync(pr => pr.Status == "Pending" && pr.IsActive),
                ApprovedRequests = await _context.PurchaseRequests.CountAsync(pr => pr.Status == "Approved" && pr.IsActive),
                RejectedRequests = await _context.PurchaseRequests.CountAsync(pr => pr.Status == "Rejected" && pr.IsActive),
                ThisMonthRequests = await _context.PurchaseRequests.CountAsync(pr =>
                    pr.RequestDate.Year == DateTime.Now.Year &&
                    pr.RequestDate.Month == DateTime.Now.Month &&
                    pr.IsActive)
            };

            return View(stats);
        }

        private string GenerateRFQNumber()
        {
            var count = _context.RequestsForQuotation.Count(r => r.CreatedDate.Year == DateTime.Now.Year) + 1;
            return $"RFQ-{DateTime.Now:yyyy}-{count:D3}";
        }

        private bool PurchaseRequestExists(int id)
        {
            return _context.PurchaseRequests.Any(e => e.PurchaseRequestID == id);
        }
    }
}



//using FridgeManagementSystem.Data;
//using FridgeManagementSystem.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FridgeManagementSystem.Areas.PurchasingSubsystem.Controllers
//{
//    [Area("PurchasingSubsystem")]
//    public class PurchaseRequestsController : Controller
//    {
//        private readonly FridgeDbContext _context;

//        public PurchaseRequestsController(FridgeDbContext context)
//        {
//            _context = context;
//        }

//        // GET: PurchaseRequests
//        public async Task<IActionResult> Index()
//        {
//            var requests = await _context.PurchaseRequests
//                .OrderByDescending(pr => pr.RequestDate)
//                .ToListAsync();
//            return View(requests);
//        }

//        // GET: PurchaseRequests/Pending - Show only pending requests for processing
//        public async Task<IActionResult> Pending()
//        {
//            var pendingRequests = await _context.PurchaseRequests
//                .Where(pr => pr.Status == "Pending" && pr.IsActive)
//                .OrderByDescending(pr => pr.RequestDate)
//                .ToListAsync();

//            return View(pendingRequests);
//        }

//        // GET: PurchaseRequests/Approved - Show approved requests
//        public async Task<IActionResult> Approved()
//        {
//            var approvedRequests = await _context.PurchaseRequests
//                .Where(pr => pr.Status == "Approved" && pr.IsActive)
//                .OrderByDescending(pr => pr.ApprovalDate)
//                .ToListAsync();

//            return View(approvedRequests);
//        }

//        // GET: PurchaseRequests/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var purchaseRequest = await _context.PurchaseRequests
//                .FirstOrDefaultAsync(m => m.PurchaseRequestID == id);
//            if (purchaseRequest == null)
//            {
//                return NotFound();
//            }

//            return View(purchaseRequest);
//        }

//        // GET: PurchaseRequests/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: PurchaseRequests/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("PurchaseRequestID,InventoryID,RequestDate,ItemFullNames,RequestBy,AssignedToRole,Status,RequestType,RequestNumber,Quantity")] PurchaseRequest purchaseRequest)
//        {
//            if (ModelState.IsValid)
//            {
//                // Set default values
//                purchaseRequest.Status = "Pending";
//                purchaseRequest.IsActive = true;
//                purchaseRequest.IsViewed = false;

//                _context.Add(purchaseRequest);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(purchaseRequest);
//        }

//        // GET: PurchaseRequests/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var purchaseRequest = await _context.PurchaseRequests.FindAsync(id);
//            if (purchaseRequest == null)
//            {
//                return NotFound();
//            }
//            return View(purchaseRequest);
//        }

//        // POST: PurchaseRequests/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("PurchaseRequestID,InventoryID,RequestDate,ItemFullNames,RequestBy,AssignedToRole,Status,RequestType,RequestNumber,Quantity,IsActive")] PurchaseRequest purchaseRequest)
//        {
//            if (id != purchaseRequest.PurchaseRequestID)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(purchaseRequest);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!PurchaseRequestExists(purchaseRequest.PurchaseRequestID))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(purchaseRequest);
//        }

//        // GET: PurchaseRequests/Process/5 - Process a specific request
//        public async Task<IActionResult> Process(int? id)
//        {
//            if (id == null) return NotFound();

//            var purchaseRequest = await _context.PurchaseRequests
//                .FirstOrDefaultAsync(pr => pr.PurchaseRequestID == id);

//            if (purchaseRequest == null) return NotFound();

//            // Mark as viewed
//            if (!purchaseRequest.IsViewed)
//            {
//                purchaseRequest.IsViewed = true;
//                purchaseRequest.ViewedDate = DateTime.Now;
//                await _context.SaveChangesAsync();
//            }

//            // Get active suppliers for the RFQ
//            ViewBag.Suppliers = await _context.Suppliers
//                .Where(s => s.IsActive)
//                .Select(s => new SelectListItem
//                {
//                    Value = s.SupplierID.ToString(),
//                    Text = $"{s.Name} - {s.Email}"
//                })
//                .ToListAsync();

//            return View(purchaseRequest);
//        }

//        // POST: PurchaseRequests/Process/5 - Approve and create RFQ
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Process(int id, string approvalNotes, int[] selectedSuppliers, string action)
//        {
//            var purchaseRequest = await _context.PurchaseRequests.FindAsync(id);
//            if (purchaseRequest == null) return NotFound();

//            if (action == "approve")
//            {
//                // 1. Update Purchase Request
//                purchaseRequest.Status = "Approved";
//                purchaseRequest.ApprovalDate = DateTime.Now;
//                purchaseRequest.ApprovalNotes = approvalNotes;

//                // 2. Create RFQ for each selected supplier
//                if (selectedSuppliers != null && selectedSuppliers.Length > 0)
//                {
//                    foreach (var supplierId in selectedSuppliers)
//                    {
//                        var rfq = new RequestForQuotation
//                        {
//                            RFQNumber = GenerateRFQNumber(),
//                            PurchaseRequestID = purchaseRequest.PurchaseRequestID,
//                            Description = $"RFQ for {purchaseRequest.Quantity} x {purchaseRequest.ItemFullNames}",
//                            RequiredQuantity = purchaseRequest.Quantity,
//                            CreatedDate = DateTime.Now,
//                            Deadline = DateTime.Now.AddDays(7), // 7 days to respond
//                            Status = "Sent"
//                        };

//                        _context.RequestsForQuotation.Add(rfq);
//                    }

//                    await _context.SaveChangesAsync();
//                    TempData["SuccessMessage"] = $"Purchase request approved and {selectedSuppliers.Length} RFQ(s) created!";
//                }
//                else
//                {
//                    TempData["ErrorMessage"] = "Please select at least one supplier.";
//                    return RedirectToAction(nameof(Process), new { id });
//                }
//            }
//            else if (action == "reject")
//            {
//                purchaseRequest.Status = "Rejected";
//                purchaseRequest.ApprovalNotes = approvalNotes;
//                await _context.SaveChangesAsync();
//                TempData["SuccessMessage"] = "Purchase request rejected.";
//            }

//            return RedirectToAction(nameof(Pending));
//        }

//        // GET: PurchaseRequests/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var purchaseRequest = await _context.PurchaseRequests
//                .FirstOrDefaultAsync(m => m.PurchaseRequestID == id);
//            if (purchaseRequest == null)
//            {
//                return NotFound();
//            }

//            return View(purchaseRequest);
//        }

//        // POST: PurchaseRequests/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var purchaseRequest = await _context.PurchaseRequests.FindAsync(id);
//            if (purchaseRequest != null)
//            {
//                // Soft delete - set IsActive to false instead of removing
//                purchaseRequest.IsActive = false;
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToAction(nameof(Index));
//        }

//        private bool PurchaseRequestExists(int id)
//        {
//            return _context.PurchaseRequests.Any(e => e.PurchaseRequestID == id);
//        }

//        private string GenerateRFQNumber()
//        {
//            var count = _context.RequestsForQuotation.Count(r => r.CreatedDate.Year == DateTime.Now.Year) + 1;
//            return $"RFQ-{DateTime.Now:yyyy}-{count:D3}";
//        }
//    }
//}
















//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using FridgeManagementSystem.Data;

//using FridgeManagementSystem.Models;
//#nullable disable
//namespace PurchasingSubsystem.Controllers
//{
//    public class PurchaseRequestsController : Controller
//    {
//        private readonly FridgeDbContext _context;

//        public PurchaseRequestsController(FridgeDbContext context)
//        {
//            _context = context;
//        }

//        // GET: PurchaseRequests
//        public async Task<IActionResult> Index()
//        {
//            return View(await _context.PurchaseRequests.ToListAsync());
//        }

//        // GET: PurchaseRequests/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var purchaseRequest = await _context.PurchaseRequests
//                .FirstOrDefaultAsync(m => m.PurchaseRequestID == id);
//            if (purchaseRequest == null)
//            {
//                return NotFound();
//            }

//            return View(purchaseRequest);
//        }

//        // GET: PurchaseRequests/Create
//        public IActionResult Create()
//        {
//            //ViewBag.InventoryLiaisons = new SelectList(_context.InventoryLiaisons, "InventoryLiaisonID", "Name");
//            return View();
//        }

//        // POST: PurchaseRequests/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("PurchaseRequestID,InventoryLiaisonId,RequestDate,Status")] PurchaseRequest purchaseRequest)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(purchaseRequest);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(purchaseRequest);
//        }

//        // GET: PurchaseRequests/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var purchaseRequest = await _context.PurchaseRequests.FindAsync(id);
//            if (purchaseRequest == null)
//            {
//                return NotFound();
//            }
//            //ViewBag.InventoryLiaisons = new SelectList(_context.InventoryLiaisons, "InventoryLiaisonID", "Name");
//            return View(purchaseRequest);
//        }

//        // POST: PurchaseRequests/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("PurchaseRequestID,InventoryLiaisonId,RequestDate,Status")] PurchaseRequest purchaseRequest)
//        {
//            if (id != purchaseRequest.PurchaseRequestID)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(purchaseRequest);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!PurchaseRequestExists(purchaseRequest.PurchaseRequestID))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(purchaseRequest);
//        }

//        // GET: PurchaseRequests/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var purchaseRequest = await _context.PurchaseRequests
//                .FirstOrDefaultAsync(m => m.PurchaseRequestID == id);
//            if (purchaseRequest == null)
//            {
//                return NotFound();
//            }

//            return View(purchaseRequest);
//        }

//        // POST: PurchaseRequests/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var purchaseRequest = await _context.PurchaseRequests.FindAsync(id);
//            _context.PurchaseRequests.Remove(purchaseRequest);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool PurchaseRequestExists(int id)
//        {
//            return _context.PurchaseRequests.Any(e => e.PurchaseRequestID == id);
//        }



//    }
//}
