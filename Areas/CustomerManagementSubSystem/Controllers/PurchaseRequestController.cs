using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CustomerManagementSubSystem.Controllers
{
    [Area("CustomerManagementSubSystem")]
    public class PurchaseRequestController : Controller
    {
        private readonly FridgeDbContext _context;

        public PurchaseRequestController(FridgeDbContext context)
        {
            _context = context;
        }

        // Check stock before creating purchase request
        public IActionResult CheckStock(int fridgeId)
        {
            var fridge = _context.Fridge.Find(fridgeId);

            if (fridge == null) return NotFound();

            if (fridge.Quantity < 5) // threshold
            {
                TempData["StockLow"] = "Stock is low! Please create a purchase request.";
                return RedirectToAction("Create", new { fridgeId = fridgeId });
            }

            return RedirectToAction("Index", "Fridge");
        }

        // GET: Display the create purchase request form
        [HttpGet]
        public IActionResult Create(int? fridgeId)
        {
            // Generate a RequestNumber for display
            var model = new PurchaseRequest
            {
                RequestNumber = $"PR-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Substring(0, 4)}"
            };

            // Pre-select fridge if coming from CheckStock
            if (fridgeId.HasValue)
            {
                model.FridgeId = fridgeId.Value;
            }

            // Populate dropdowns
            PopulateDropdowns(model.FridgeId);

            return View(model);
        }

        // POST: Handle form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseRequest request)
        {
            if (ModelState.IsValid)
            {
                // Ensure unique RequestNumber in database
                request.RequestNumber = $"PR-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Substring(0, 4)}";
                request.RequestDate = DateOnly.FromDateTime(DateTime.Now);
                request.Status = "Pending";

                _context.PurchaseRequests.Add(request);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Purchase request submitted successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdowns if model state is invalid
            PopulateDropdowns(request.FridgeId);

            return View(request);
        }

        // Optional: Index action to display all purchase requests
        public IActionResult Index()
        {
            var requests = _context.PurchaseRequests
                .OrderByDescending(r => r.RequestDate)
                .ToList();
            return View(requests);
        }

        // Helper method to populate dropdowns
        private void PopulateDropdowns(int? selectedCustomerId = null, int? selectedFridgeId = null)
        {
            ViewBag.Customers = new SelectList(_context.Customers.Where(c => c.IsActive), "CustomerID", "FullNames", selectedCustomerId);
            ViewBag.Fridges = new SelectList(_context.Fridge, "FridgeId", "FridgeName", selectedFridgeId);
        }
    }
}
