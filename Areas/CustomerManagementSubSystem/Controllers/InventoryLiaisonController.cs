using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagementSubSystem.Models; 

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
        // GET
        public async Task<IActionResult> Receive(int? fridgeId)
        {
            if (fridgeId == null)
                return NotFound();

            var fridge = await _context.Fridge.FindAsync(fridgeId.Value);
            if (fridge == null)
                return NotFound();

            // Find the approved purchase request for this fridge
            var request = await _context.PurchaseRequests
                .FirstOrDefaultAsync(r => r.FridgeId == fridgeId && r.Status == "Approved");

            // Use requested quantity if fridge quantity is 0
            int initialQuantity = fridge.Quantity > 0 ? fridge.Quantity : request?.Quantity ?? 1;

            // Populate ViewModel
            var model = new ReceiveFridgeVm
            {
                FridgeId = fridge.FridgeId,
                Brand = fridge.Brand,
                Model = fridge.Model,
                Type = fridge.FridgeType,
                SerialNumber = fridge.SerialNumber,
                SupplierId = fridge.SupplierID,
                Quantity = initialQuantity,
                DateAdded = fridge.DateAdded.ToDateTime(TimeOnly.MinValue)
            };

            ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "Name", fridge.SupplierID);
            return View(model);
        }


        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(ReceiveFridgeVm model)
        {
            if (!ModelState.IsValid)
            {
                // ✅ Re-populate readonly fields from database
                var fridgeData = await _context.Fridge.FindAsync(model.FridgeId);
                if (fridgeData != null)
                {
                    model.Brand = fridgeData.Brand;
                    model.Model = fridgeData.Model;
                    model.Type = fridgeData.FridgeType;
                }

                ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "Name", model.SupplierId);
                return View(model);
            }

            var fridge = await _context.Fridge.FindAsync(model.FridgeId);
            if (fridge == null)
                return NotFound();

            // Update fridge details
            fridge.Quantity = model.Quantity;
            fridge.Status = "Available";
            fridge.SupplierID = model.SupplierId;
            fridge.SerialNumber = model.SerialNumber; // make sure serial number is updated
            fridge.UpdatedDate = DateTime.Now;
            fridge.IsActive = true;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Fridge received and marked as Available!";
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
        public async Task<IActionResult> CreatePurchaseRequest(int? fridgeId)
        {
            var model = new CreatePurchaseRequestViewModel();

            if (fridgeId != null)
            {
                var fridge = await _context.Fridge
                    .FirstOrDefaultAsync(f => f.FridgeId == fridgeId);

                if (fridge != null)
                {
                    model.FridgeId = fridge.FridgeId;
                    model.ItemFullNames = $"{fridge.Brand} - {fridge.FridgeType}";
                }
            }

            return View(model);
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

            // --------------------------
            // Ensure the fridge exists if fridgeId is provided
            // --------------------------
            Fridge? linkedFridge = null;
            if (model.FridgeId.HasValue)
            {
                linkedFridge = await _context.Fridge
                    .FirstOrDefaultAsync(f => f.FridgeId == model.FridgeId.Value);

                if (linkedFridge != null)
                {
                    // Make sure ItemFullNames is consistent
                    model.ItemFullNames = $"{linkedFridge.Brand} - {linkedFridge.FridgeType}";
                }
                else
                {
                    // If fridgeId was provided but not found, clear it to avoid null issues
                    model.FridgeId = null;
                }
            }

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
                FridgeId = linkedFridge?.FridgeId, // link fridge only if it exists
                InventoryID = model.InventoryID != 0 ? model.InventoryID : (int?)null
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
                .Include(r => r.Fridge)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            var viewModel = requests.Select(r => new PurchaseRequestViewModel
            {
                PurchaseRequestID = r.PurchaseRequestID,
                RequestDate = r.RequestDate,
                Status = r.Status,
                Quantity = r.Quantity,
                ItemFullNames = r.ItemFullNames,
                RequestNumber = r.RequestNumber,
                FridgeId = r.FridgeId,
                Fridge = r.Fridge
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

