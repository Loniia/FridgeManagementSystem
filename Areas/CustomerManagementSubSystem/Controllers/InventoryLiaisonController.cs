using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Areas.CustomerManagementSubSystem.Controllers
{
    [Area("CustomerManagementSubSystem")]
    public class InventoryLiaisonController : Controller
    {
        private readonly FridgeDbContext _context;
        private const int LowStockThreshold = 5; // Low stock threshold

        public InventoryLiaisonController(FridgeDbContext context)
        {
            _context = context;
        }

        // --------------------------
        // View Fridge Stock
        // --------------------------
        public async Task<IActionResult> Index()
        {
            var fridges = await _context.Fridge
                .Include(f => f.FridgeAllocation)
                    .ThenInclude(a => a.Customer)
                // ✅ Show all fridges that are already in your subsystem (Received or Available)
                .Where(f => f.Status == "Received" || f.Status == "Available" || f.Status == "Damaged" || f.Status == "Scrapped")
                .Select(f => new FridgeViewModel
                {
                    FridgeId = f.FridgeId,
                    FridgeType = f.FridgeType,
                    Brand = f.Brand,
                    Model = f.Model,
                    SerialNumber = f.SerialNumber,
                    Condition = f.Condition,
                    Status = f.Status,
                    Quantity = f.Quantity,

                    // Stock availability calculation
                    AvailableStock = f.Quantity - f.FridgeAllocation
                        .Where(a => a.ReturnDate == null || a.ReturnDate > DateOnly.FromDateTime(DateTime.Today))
                        .Sum(a => 1),

                    CustomerName = f.FridgeAllocation
                        .OrderByDescending(a => a.AllocationDate)
                        .Select(a => a.Customer.FullName)
                        .FirstOrDefault(),

                    AllocationDate = f.FridgeAllocation
                        .OrderByDescending(a => a.AllocationDate)
                        .Select(a => (DateOnly?)a.AllocationDate)
                        .FirstOrDefault(),

                    ReturnDate = f.FridgeAllocation
                        .OrderByDescending(a => a.AllocationDate)
                        .Select(a => (DateOnly?)a.ReturnDate)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return View(fridges);
        }

        // --------------------------
        // Receive New Fridges
        // --------------------------
        public async Task<IActionResult> Receive()
        {
            ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(Fridge fridge)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "Name", fridge.SupplierID);
                return View(fridge);
            }

            // Set automatic values
            fridge.Status = "Received";
            fridge.DateAdded = DateOnly.FromDateTime(DateTime.Now);
            fridge.UpdatedDate = DateTime.Now;
            fridge.IsActive = true;

            _context.Fridge.Add(fridge);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Fridge received successfully!";
            return RedirectToAction(nameof(Index));
        }

        // --------------------------
        // Update Fridge Status
        // --------------------------
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var fridge = await _context.Fridge
                .FindAsync(id);

            if (fridge == null) return NotFound();

            ViewBag.StatusOptions = new SelectList(new List<string> { "Available", "Damaged", "Scrapped" }, fridge.Status);
            return View(fridge);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(Fridge updatedFridge)
        {
            if (updatedFridge == null || updatedFridge.FridgeId == 0) return NotFound();

            var fridge = await _context.Fridge
                .FindAsync(updatedFridge.FridgeId);

            if (fridge == null) return NotFound();

            fridge.Status = updatedFridge.Status;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Fridge '{fridge.Brand} {fridge.Model}' status updated to '{fridge.Status}'!";
            return RedirectToAction(nameof(Index));
        }

        // --------------------------
        // Check Stock and Auto Create Purchase Request
        // --------------------------
        public async Task<IActionResult> CheckStock()
        {
            int threshold = 5;
            int availableCount = await _context.Fridge.CountAsync(f => f.Status == "Available" && f.IsActive);

            if (availableCount < threshold)
            {
                var purchaseRequest = new PurchaseRequest
                {
                    RequestDate = DateOnly.FromDateTime(DateTime.Now),
                    Status = "Pending",
                    Quantity = threshold - availableCount,
                    ItemFullNames = "Fridge",
                    IsActive = true,
                    AssignedToRole = "PurchasingManager",
                    RequestBy = "InventoryLiaison",
                    RequestType = "Fridge Purchase"
                };
                
                var year = DateTime.Now.Year;
                int countForYear = await _context.PurchaseRequests.CountAsync(r => r.RequestDate.Year == year) + 1;
                purchaseRequest.RequestNumber = $"PR-{year}-{countForYear:D3}";

                _context.PurchaseRequests.Add(purchaseRequest);
                await _context.SaveChangesAsync();

                TempData["InfoMessage"] = $"Stock is low ({availableCount} available). Purchase request {purchaseRequest.RequestNumber} created!";
            }
            else
            {
                TempData["InfoMessage"] = $"Stock is sufficient ({availableCount} available).";
            }

            return RedirectToAction(nameof(CreatePurchaseRequest));
        }

        // --------------------------
        // Create Purchase Request (GET)
        // --------------------------
        [HttpGet]
        public IActionResult CreatePurchaseRequest()
        {
            return View();
        }

        // --------------------------
        // Create Purchase Request (POST)
        // --------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePurchaseRequest(CreatePurchaseRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var newRequest = new PurchaseRequest
            {
                ItemFullNames = model.ItemFullNames,
                Quantity = model.Quantity,
                RequestBy = "Inventory Liaison",
                RequestType = "Fridge Purchase",
                AssignedToRole = EmployeeRoles.PurchasingManager,
                RequestDate = DateOnly.FromDateTime(DateTime.Now),
                Status = "Pending",
                IsActive = true,
                FridgeId = model.FridgeId, // optional link (nullable)
                InventoryID = model.InventoryID != 0 ? model.InventoryID : (int?)null // only set if from stock
            };

            // Generate unique Request Number
            var year = DateTime.Now.Year;
            int countForYear = await _context.PurchaseRequests
                .CountAsync(r => r.RequestDate.Year == year) + 1;

            newRequest.RequestNumber = $"PR-{year}-{countForYear:D3}";

            _context.PurchaseRequests.Add(newRequest);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Purchase request {newRequest.RequestNumber} created successfully!";
            return RedirectToAction(nameof(ProcessPurchaseRequests));
        }


        // --------------------------
        // Process Purchase Requests (List)
        // --------------------------
        public async Task<IActionResult> ProcessPurchaseRequests()
        {
            var requests = await _context.PurchaseRequests
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            var viewModel = requests.Select(r => new PurchaseRequestViewModel
            {
                PurchaseRequestID = r.PurchaseRequestID,
                RequestDate = r.RequestDate,
                Status = r.Status,
                Quantity = r.Quantity,
                ItemFullNames = r.ItemFullNames,
                RequestNumber = r.RequestNumber
            }).ToList();

            return View(viewModel);
        }

       
            // Monthly Dashboard View
            // --------------------------
            public async Task<IActionResult> MonthlyDashboard(int? year)
            {
                int selectedYear = year ?? DateTime.Now.Year;

                // Prepare data for all months
                var months = Enumerable.Range(1, 12).Select(m => new DateTime(selectedYear, m, 1).ToString("MMMM")).ToList();
                var receivedCounts = new List<int>();
                var receivedColors = new List<string>();
                var allocatedCounts = new List<int>();
                var returnedCounts = new List<int>();

                for (int month = 1; month <= 12; month++)
                {
                    int received = await _context.Fridge.CountAsync(f => f.DateAdded.Year == selectedYear && f.DateAdded.Month == month);
                    int allocated = await _context.FridgeAllocation.CountAsync(a => a.AllocationDate.Year == selectedYear && a.AllocationDate.Month == month && a.Status == "Allocated");
                    int returned = await _context.FridgeAllocation.CountAsync(a => a.ReturnDate.HasValue && a.ReturnDate.Value.Year == selectedYear && a.ReturnDate.Value.Month == month);

                    receivedCounts.Add(received);
                    allocatedCounts.Add(allocated);
                    returnedCounts.Add(returned);

                    // Color-code received fridges
                    receivedColors.Add(received < LowStockThreshold ? "rgba(255, 99, 132, 0.6)" : "rgba(54, 162, 235, 0.6)");
                }

                // Summary totals
                ViewBag.TotalReceived = receivedCounts.Sum();
                ViewBag.TotalAllocated = allocatedCounts.Sum();
                ViewBag.TotalReturned = returnedCounts.Sum();
                ViewBag.LowStockMonths = receivedCounts.Count(c => c < LowStockThreshold);
                ViewBag.SelectedYear = selectedYear;

                // Pass to view
                var model = new MonthlyDashboardViewModel
                {
                    Months = months,
                    ReceivedCounts = receivedCounts,
                    ReceivedColors = receivedColors,
                    AllocatedCounts = allocatedCounts,
                    ReturnedCounts = returnedCounts
                };

                return View(model);
            }


            // --------------------------
            // AJAX Endpoint for Year Selection
            // --------------------------
            [HttpGet]
            public async Task<IActionResult> GetMonthlyStats(int year)
            {
                var months = Enumerable.Range(1, 12).Select(m => new DateTime(year, m, 1).ToString("MMMM")).ToList();
                var receivedCounts = new List<int>();
                var receivedColors = new List<string>();
                var allocatedCounts = new List<int>();
                var returnedCounts = new List<int>();

                for (int month = 1; month <= 12; month++)
                {
                    int received = await _context.Fridge.CountAsync(f => f.DateAdded.Year == year && f.DateAdded.Month == month);
                    int allocated = await _context.FridgeAllocation.CountAsync(a => a.AllocationDate.Year == year && a.AllocationDate.Month == month && a.Status == "Allocated");
                    int returned = await _context.FridgeAllocation.CountAsync(a => a.ReturnDate.HasValue && a.ReturnDate.Value.Year == year && a.ReturnDate.Value.Month == month);

                    receivedCounts.Add(received);
                    allocatedCounts.Add(allocated);
                    returnedCounts.Add(returned);

                    receivedColors.Add(received < LowStockThreshold ? "rgba(255, 99, 132, 0.6)" : "rgba(54, 162, 235, 0.6)");
                }

                return Json(new
                {
                    months,
                    receivedCounts,
                    receivedColors,
                    allocatedCounts,
                    returnedCounts,
                    totalReceived = receivedCounts.Sum(),
                    totalAllocated = allocatedCounts.Sum(),
                    totalReturned = returnedCounts.Sum(),
                    lowStockMonths = receivedCounts.Count(c => c < LowStockThreshold)
                });

            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var fridge = _context.Fridge.FirstOrDefault(f => f.FridgeId == id);
            if (fridge != null)
            {
                fridge.IsActive = false; // Soft delete instead of removing
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Fridge removed successfully.";
            }
            return RedirectToAction(nameof(Index)); // Go back to inventory list
        }
    }
}

