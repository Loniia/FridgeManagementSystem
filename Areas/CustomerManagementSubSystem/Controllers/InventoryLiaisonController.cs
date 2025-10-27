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

        // GET - Modified to handle both direct fridge receiving AND purchase request flow
        // GET - Fixed version
        public async Task<IActionResult> Receive(int? fridgeId, int? purchaseRequestId)
        {
            ReceiveFridgeVm model;

            // If coming from approved purchase request
            if (purchaseRequestId.HasValue)
            {
                var purchaseRequest = await _context.PurchaseRequests
                    .Include(pr => pr.Fridge) // Include fridge details
                    .FirstOrDefaultAsync(pr => pr.PurchaseRequestID == purchaseRequestId && pr.Status == "Approved");

                if (purchaseRequest == null)
                {
                    TempData["ErrorMessage"] = "Approved purchase request not found!";
                    return RedirectToAction(nameof(ProcessPurchaseRequests));
                }

                // DEBUG: Check what we're getting
                System.Diagnostics.Debug.WriteLine($"Purchase Request: {purchaseRequest.PurchaseRequestID}, Fridge: {purchaseRequest.FridgeId}");

                // If there's a linked fridge, use its details
                if (purchaseRequest.Fridge != null)
                {
                    model = new ReceiveFridgeVm
                    {
                        PurchaseRequestID = purchaseRequest.PurchaseRequestID,
                        FridgeId = purchaseRequest.Fridge.FridgeId,
                        Brand = purchaseRequest.Fridge.Brand,
                        Model = purchaseRequest.Fridge.Model,
                        Type = purchaseRequest.Fridge.FridgeType,
                        SerialNumber = purchaseRequest.Fridge.SerialNumber,
                        SupplierId = purchaseRequest.Fridge.SupplierID,
                        Quantity = purchaseRequest.Quantity,
                        DateAdded = DateTime.Now,
                        Status = "Received"
                    };
                }
                else
                {
                    // If no fridge linked, extract details from ItemFullNames
                    string brand = "Standard Brand";
                    string modelName = "Standard Model";
                    string type = "Standard Type";

                    if (!string.IsNullOrEmpty(purchaseRequest.ItemFullNames))
                    {
                        var parts = purchaseRequest.ItemFullNames.Split('-');
                        if (parts.Length >= 2)
                        {
                            brand = parts[0].Trim();
                            type = parts[1].Trim();
                        }
                        else
                        {
                            type = purchaseRequest.ItemFullNames.Trim();
                        }
                    }

                    model = new ReceiveFridgeVm
                    {
                        PurchaseRequestID = purchaseRequest.PurchaseRequestID,
                        FridgeId = 0, // Will create new fridge
                        Brand = brand,
                        Model = modelName,
                        Type = type,
                        SerialNumber = $"SN-{DateTime.Now:yyyyMMdd-HHmmss}",
                        SupplierId = 1, // Default supplier
                        Quantity = purchaseRequest.Quantity,
                        DateAdded = DateTime.Now,
                        Status = "Received"
                    };
                }
            }
            // If coming directly with fridgeId (existing flow)
            else if (fridgeId.HasValue && fridgeId > 0)
            {
                var fridge = await _context.Fridge.FindAsync(fridgeId.Value);
                if (fridge == null)
                    return NotFound();

                model = new ReceiveFridgeVm
                {
                    FridgeId = fridge.FridgeId,
                    Brand = fridge.Brand,
                    Model = fridge.Model,
                    Type = fridge.FridgeType,
                    SerialNumber = fridge.SerialNumber,
                    SupplierId = fridge.SupplierID,
                    Quantity = fridge.Quantity,
                    DateAdded = DateTime.Now,
                    Status = "Received"
                };
            }
            else
            {
                TempData["ErrorMessage"] = "No valid fridge or purchase request specified!";
                return RedirectToAction(nameof(Index));
            }

            // Ensure we have valid data
            if (string.IsNullOrEmpty(model.Brand)) model.Brand = "Standard Brand";
            if (string.IsNullOrEmpty(model.Model)) model.Model = "Standard Model";
            if (string.IsNullOrEmpty(model.Type)) model.Type = "Standard Type";
            if (string.IsNullOrEmpty(model.SerialNumber)) model.SerialNumber = $"SN-{DateTime.Now:yyyyMMdd-HHmmss}";

            ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "Name", model.SupplierId);
            ViewBag.StatusOptions = new SelectList(new List<string> { "Received", "Available" }, model.Status);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(ReceiveFridgeVm model)
        {
            // 🔥 CRITICAL FIX: If Brand/Model/Type are empty but we have FridgeId, get them from database
            if (model.FridgeId > 0 && (string.IsNullOrEmpty(model.Brand) || string.IsNullOrEmpty(model.Model) || string.IsNullOrEmpty(model.Type)))
            {
                var existingFridge = await _context.Fridge.FindAsync(model.FridgeId);
                if (existingFridge != null)
                {
                    model.Brand = existingFridge.Brand;
                    model.Model = existingFridge.Model;
                    model.Type = existingFridge.FridgeType;
                }
            }

            // 🔥 ALSO: If we have PurchaseRequestID but empty fields, try to get from purchase request
            if (model.PurchaseRequestID.HasValue && (string.IsNullOrEmpty(model.Brand) || string.IsNullOrEmpty(model.Model)))
            {
                var purchaseRequest = await _context.PurchaseRequests
                    .Include(pr => pr.Fridge)
                    .FirstOrDefaultAsync(pr => pr.PurchaseRequestID == model.PurchaseRequestID.Value);

                if (purchaseRequest?.Fridge != null)
                {
                    model.Brand = purchaseRequest.Fridge.Brand;
                    model.Model = purchaseRequest.Fridge.Model;
                    model.Type = purchaseRequest.Fridge.FridgeType;
                    model.FridgeId = purchaseRequest.Fridge.FridgeId; // Ensure FridgeId is set
                }
                else if (!string.IsNullOrEmpty(purchaseRequest?.ItemFullNames))
                {
                    // Extract from ItemFullNames as fallback
                    var parts = purchaseRequest.ItemFullNames.Split('-');
                    if (parts.Length >= 2)
                    {
                        model.Brand = parts[0].Trim();
                        model.Type = parts[1].Trim();
                        model.Model = "Standard Model";
                    }
                }
            }

            // Set default values if still empty (safety net)
            if (string.IsNullOrEmpty(model.Brand)) model.Brand = "Standard Brand";
            if (string.IsNullOrEmpty(model.Model)) model.Model = "Standard Model";
            if (string.IsNullOrEmpty(model.Type)) model.Type = "Standard Type";
            if (string.IsNullOrEmpty(model.SerialNumber)) model.SerialNumber = $"SN-{DateTime.Now:yyyyMMdd-HHmmss}";

            // Now validate the model
            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "Name", model.SupplierId);
                ViewBag.StatusOptions = new SelectList(new List<string> { "Received", "Available" }, model.Status);
                return View(model);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Find or create fridge
                var fridge = await _context.Fridge.FindAsync(model.FridgeId);

                if (fridge == null && model.FridgeId > 0)
                {
                    // Try to find by other criteria if FridgeId doesn't exist
                    fridge = await _context.Fridge
                        .FirstOrDefaultAsync(f => f.Brand == model.Brand && f.Model == model.Model && f.FridgeType == model.Type);
                }

                if (fridge == null)
                {
                    // Create new fridge if doesn't exist
                    fridge = new Fridge
                    {
                        FridgeType = model.Type,
                        Brand = model.Brand,
                        Model = model.Model,
                        SerialNumber = model.SerialNumber,
                        Quantity = model.Quantity,
                        SupplierID = model.SupplierId,
                        Status = model.Status, // This is key - sets to Available
                        DateAdded = DateOnly.FromDateTime(model.DateAdded),
                        UpdatedDate = DateTime.Now,
                        IsActive = true,
                        Condition = "New" // Add default condition
                    };
                    _context.Fridge.Add(fridge);
                }
                else
                {
                    // Update existing fridge - INCREASE quantity, don't replace it
                    fridge.Quantity += model.Quantity; // Add to existing quantity
                    fridge.Status = model.Status; // This is key - sets to Available
                    fridge.SupplierID = model.SupplierId;

                    // Only update serial number if provided
                    if (!string.IsNullOrEmpty(model.SerialNumber) && model.SerialNumber != fridge.SerialNumber)
                    {
                        fridge.SerialNumber = model.SerialNumber;
                    }

                    fridge.UpdatedDate = DateTime.Now;
                    fridge.IsActive = true;
                }

                await _context.SaveChangesAsync();

                // If this came from a purchase request, update its status
                if (model.PurchaseRequestID.HasValue)
                {
                    var purchaseRequest = await _context.PurchaseRequests
                        .FindAsync(model.PurchaseRequestID.Value);

                    if (purchaseRequest != null)
                    {
                        purchaseRequest.Status = "Completed";
                        // Link the fridge to the purchase request if not already linked
                        if (purchaseRequest.FridgeId == null)
                        {
                            purchaseRequest.FridgeId = fridge.FridgeId;
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();

                TempData["SuccessMessage"] = $"Fridge received and marked as {model.Status}! Stock updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "Error receiving fridge: " + ex.Message;
                ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "Name", model.SupplierId);
                ViewBag.StatusOptions = new SelectList(new List<string> { "Received", "Available" }, model.Status);
                return View(model);
            }
        }

        //// POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Receive(ReceiveFridgeVm model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // ✅ Re-populate readonly fields from database
        //        var fridgeData = await _context.Fridge.FindAsync(model.FridgeId);
        //        if (fridgeData != null)
        //        {
        //            model.Brand = fridgeData.Brand;
        //            model.Model = fridgeData.Model;
        //            model.Type = fridgeData.FridgeType;
        //        }

        //        ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "Name", model.SupplierId);
        //        return View(model);
        //    }

        //    var fridge = await _context.Fridge.FindAsync(model.FridgeId);
        //    if (fridge == null)
        //        return NotFound();

        //    // Update fridge details
        //    fridge.Quantity = model.Quantity;
        //    fridge.Status = "Available";
        //    fridge.SupplierID = model.SupplierId;
        //    fridge.SerialNumber = model.SerialNumber; // make sure serial number is updated
        //    fridge.UpdatedDate = DateTime.Now;
        //    fridge.IsActive = true;

        //    await _context.SaveChangesAsync();

        //    TempData["SuccessMessage"] = "Fridge received and marked as Available!";
        //    return RedirectToAction(nameof(Index));
        //}

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

