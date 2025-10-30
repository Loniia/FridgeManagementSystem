using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Utilities;
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
                // ✅ Show all fridges that are already in your subsystem (Received, Available, Damaged, Scrapped)
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

                    // Take the latest allocation for CustomerName, AllocationDate, and ReturnDate
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


        // GET - Receive New Fridges
        public async Task<IActionResult> Receive(int? fridgeId, int? purchaseRequestId)
        {
            ReceiveFridgeVm model;

            // If coming from approved purchase request
            if (purchaseRequestId.HasValue)
            {
                var purchaseRequest = await _context.PurchaseRequests
                    .Include(pr => pr.Fridge)
                    .FirstOrDefaultAsync(pr => pr.PurchaseRequestID == purchaseRequestId && pr.Status == "Approved");

                if (purchaseRequest == null)
                {
                    TempData["ErrorMessage"] = "Approved purchase request not found!";
                    return RedirectToAction(nameof(ProcessPurchaseRequests));
                }

                // Use the request date if it exists, else today
                DateOnly dateAdded = purchaseRequest.RequestDate ?? DateOnly.FromDateTime(DateTime.Today);

                if (purchaseRequest.Fridge != null)
                {
                    model = new ReceiveFridgeVm
                    {
                        PurchaseRequestID = purchaseRequest.PurchaseRequestID,
                        FridgeId = purchaseRequest.Fridge.FridgeId,
                        Brand = purchaseRequest.Fridge.Brand,
                        ModelName = purchaseRequest.Fridge.Model,
                        Type = purchaseRequest.Fridge.FridgeType,
                        SerialNumber = purchaseRequest.Fridge.SerialNumber,
                        SupplierId = purchaseRequest.Fridge.SupplierID,
                        Quantity = purchaseRequest.Quantity,
                        DateAdded = dateAdded,
                        Status = "Received"
                    };
                }
                else
                {
                    // Extract from ItemFullNames if no fridge linked
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
                        FridgeId = 0,
                        Brand = brand,
                        ModelName = modelName,
                        Type = type,
                        SerialNumber = SerialNumberGenerator.GenerateFridgeSerialNumber(brand, type, 0),
                        SupplierId = 1,
                        Quantity = purchaseRequest.Quantity,
                        DateAdded = dateAdded,
                        Status = "Received"
                    };
                }
            }
            // Direct fridge flow
            else if (fridgeId.HasValue && fridgeId > 0)
            {
                var fridge = await _context.Fridge.FindAsync(fridgeId.Value);
                if (fridge == null) return NotFound();

                DateOnly dateAdded = fridge.DateAdded ?? DateOnly.FromDateTime(DateTime.Today);

                model = new ReceiveFridgeVm
                {
                    FridgeId = fridge.FridgeId,
                    Brand = fridge.Brand,
                    ModelName = fridge.Model,
                    Type = fridge.FridgeType,
                    SerialNumber = fridge.SerialNumber,
                    SupplierId = fridge.SupplierID,
                    Quantity = fridge.Quantity,
                    DateAdded = dateAdded,
                    Status = "Received"
                };
            }
            else
            {
                TempData["ErrorMessage"] = "No valid fridge or purchase request specified!";
                return RedirectToAction(nameof(Index));
            }

            // Default values safety net
            if (string.IsNullOrEmpty(model.Brand)) model.Brand = "Standard Brand";
            if (string.IsNullOrEmpty(model.ModelName)) model.ModelName = "Standard Model";
            if (string.IsNullOrEmpty(model.Type)) model.Type = "Standard Type";
            if (string.IsNullOrEmpty(model.SerialNumber))
                model.SerialNumber = SerialNumberGenerator.GenerateFridgeSerialNumber(model.Brand, model.Type, model.FridgeId);

            ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "Name", model.SupplierId);
            ViewBag.StatusOptions = new SelectList(new List<string> { "Received", "Available" }, model.Status);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(ReceiveFridgeVm model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "Name", model.SupplierId);
                ViewBag.StatusOptions = new SelectList(new List<string> { "Received", "Available" }, model.Status);
                TempData["ErrorMessage"] = "Please correct the errors and try again.";
                return View(model);
            }

            try
            {
                // Find fridge or create a new one
                Fridge fridge;

                if (model.FridgeId > 0)
                {
                    fridge = await _context.Fridge.FindAsync(model.FridgeId);
                    if (fridge == null)
                    {
                        TempData["ErrorMessage"] = "Fridge not found!";
                        return RedirectToAction(nameof(Index));
                    }

                    // Update existing fridge info
                    fridge.SerialNumber = model.SerialNumber;
                    fridge.Quantity = model.Quantity;
                    fridge.DateAdded = model.DateAdded ?? DateOnly.FromDateTime(DateTime.Today);
                    fridge.Status = model.Status;
                    fridge.SupplierID = model.SupplierId;
                }
                else
                {
                    // Create a new fridge record
                    fridge = new Fridge
                    {
                        Brand = model.Brand,
                        Model = model.ModelName,
                        FridgeType = model.Type,
                        SerialNumber = model.SerialNumber,
                        Quantity = model.Quantity,
                        DateAdded = model.DateAdded ?? DateOnly.FromDateTime(DateTime.Today),
                        Status = model.Status,
                        SupplierID = model.SupplierId,
                        IsActive = true
                    };
                    _context.Fridge.Add(fridge);
                }

                // Save fridge data
                await _context.SaveChangesAsync();

                // Optional: update the related purchase request to "Completed"
                if (model.PurchaseRequestID > 0)
                {
                    var request = await _context.PurchaseRequests.FindAsync(model.PurchaseRequestID);
                    if (request != null)
                    {
                        request.Status = "Completed";
                        await _context.SaveChangesAsync();
                    }
                }

                TempData["SuccessMessage"] = "Fridge successfully received!";
                return RedirectToAction("Index", "InventoryLiaison");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error receiving fridge: " + ex.Message;
                ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "Name", model.SupplierId);
                ViewBag.StatusOptions = new SelectList(new List<string> { "Received", "Available" }, model.Status);
                return View(model);
            }
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
                int countForYear = await _context.PurchaseRequests
                    .CountAsync(r => r.RequestDate.HasValue && r.RequestDate.Value.ToDateTime(TimeOnly.MinValue).Year == year) + 1;


                // ✅ USING UTILITY CLASS FOR PURCHASE REQUEST NUMBER
                purchaseRequest.RequestNumber = SerialNumberGenerator.GeneratePurchaseRequestNumber(countForYear, year);

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

            // --------------------------
            // Create new PurchaseRequest
            // --------------------------
            var newRequest = new PurchaseRequest
            {
                ItemFullNames = model.ItemFullNames,
                Quantity = model.Quantity,
                RequestBy = "Inventory Liaison",
                RequestType = "Fridge Purchase",
                AssignedToRole = EmployeeRoles.PurchasingManager,

                // ✅ FIXED: Use the date the user picked, fallback to today if blank
                // ✅ FIXED: Convert DateTime? from the model to DateOnly
                RequestDate = model.RequestDate.HasValue
                    ? DateOnly.FromDateTime(model.RequestDate.Value)
                    : DateOnly.FromDateTime(DateTime.Today),

                Status = "Pending",
                IsActive = true,
                FridgeId = linkedFridge?.FridgeId, // link fridge only if it exists
                InventoryID = model.InventoryID != 0 ? model.InventoryID : (int?)null
            };

            // --------------------------
            // Count PurchaseRequests for this year
            // --------------------------
            var currentYear = DateTime.Now.Year;
            var allRequestsWithDate = await _context.PurchaseRequests
                .Where(r => r.RequestDate.HasValue)
                .ToListAsync();

            int countForYear = allRequestsWithDate
                .Count(r => r.RequestDate.Value.Year == currentYear) + 1;

            newRequest.RequestNumber = SerialNumberGenerator.GeneratePurchaseRequestNumber(countForYear, currentYear);

            // --------------------------
            // Save new request
            // --------------------------
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fridge = await _context.Fridge.FirstOrDefaultAsync(f => f.FridgeId == id);
            if (fridge != null)
            {
                fridge.IsActive = false; // Soft delete instead of removing
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Fridge removed successfully.";
            }
            return RedirectToAction(nameof(Index)); // Go back to inventory list
        }

    }
}

