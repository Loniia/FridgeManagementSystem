using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Rotativa.AspNetCore;
using System.IO;


namespace CustomerManagementSubSystem.Controllers
{
    [Area("CustomerManagementSubSystem")]
    public class CustomerLiaisonController : Controller
    {
        private readonly FridgeDbContext _context;
        private readonly IMaintenanceRequestService _mrService;
        public CustomerLiaisonController(FridgeDbContext context, IMaintenanceRequestService mrService)
        {
            _context = context;
            _mrService = mrService;
        }

        // --------------------------
        // List Customers (Active + Inactive)
        // --------------------------
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers
                                          .IgnoreQueryFilters()
                                          .Include(c => c.FridgeAllocation)
                                          .ThenInclude(a => a.Fridge)
                                          .ToListAsync();

            var model = customers.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerID,
                FullNames = c.FullName,
                LocationId = c.LocationId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                IsActive = c.IsActive,
                RegistrationDate = c.RegistrationDate,
                FridgeAllocations = c.FridgeAllocation
                                     .Select(a => new FridgeAllocationViewModel
                                     {
                                         AllocationID = a.AllocationID,
                                         FridgeId = a.FridgeId,
                                         QuantityAllocated = a.QuantityAllocated,
                                         AllocationDate = a.AllocationDate,
                                         ReturnDate = a.ReturnDate,
                                         Status = a.Status,
                                         Fridge = new FridgeViewModel
                                         {
                                             FridgeId = a.FridgeId,
                                             Brand = a.Fridge?.Brand,
                                             Model = a.Fridge?.Model,
                                             Status = a.Fridge?.Status
                                         }
                                     }).ToList()
            }).ToList();

            return View(model);
        }

        // --------------------------
        // Create Customer
        // --------------------------
        public IActionResult Create()
        {
            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "LocationName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                // reload the dropdown if model validation fails
                ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "LocationName");
                return View(customer);
            }

            customer.IsActive = true;
            customer.RegistrationDate = DateOnly.FromDateTime(DateTime.Now);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Customer created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // --------------------------
        // Edit Customer
        // --------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null || !customer.IsActive) return NotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            try
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Customer updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Customers.Any(e => e.CustomerID == customer.CustomerID))
                    return NotFound();
                else throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // --------------------------
        // Delete Customer (Soft Delete)
        // --------------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers
                                         .FirstOrDefaultAsync(c => c.CustomerID == id && c.IsActive);

            if (customer == null) return NotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CustomerID)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            if (customer != null)
            {
                customer.IsActive = false;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Customer deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // --------------------------
        // Allocate Fridge to Customer
        // --------------------------
        public async Task<IActionResult> Allocate(int customerId)
        {
            var customer = await _context.Customers
                                         .Include(c => c.FridgeAllocation)
                                         .FirstOrDefaultAsync(c => c.CustomerID == customerId);

            if (customer == null) return NotFound();

            var model = new CustomerAllocationViewModel
            {
                CustomerId = customer.CustomerID,
                CustomerName = customer.FullName,
                Status = "Allocated",
                AvailableFridges = await _context.Fridge
                                                .Where(f => f.IsActive && f.Status == "Available" && f.Quantity > 0)
                                                .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Allocate(CustomerAllocationViewModel model)
        {
            // Validation checks
            if (model.SelectedFridgeID == 0)
                ModelState.AddModelError("", "Please select a fridge to allocate.");

            if (model.QuantityAllocated <= 0)
                ModelState.AddModelError("", "Quantity allocated must be greater than zero.");

            if (model.ReturnDate < DateOnly.FromDateTime(DateTime.Now))
                ModelState.AddModelError("", "Return date cannot be in the past.");

            if (!ModelState.IsValid)
            {
                model.AvailableFridges = _context.Fridge
                                                 .Where(f => f.IsActive && f.Status != "Scrapped" && f.Quantity > 0)
                                                 .ToList();
                return View(model);
            }

            var customer = _context.Customers.Find(model.CustomerId);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Customer not found.";
                return RedirectToAction("Index");
            }

            var fridge = _context.Fridge
                .Find(model.SelectedFridgeID);

            if (fridge == null || !fridge.IsActive || fridge.Status == "Scrapped")
            {
                TempData["ErrorMessage"] = "Selected fridge is invalid.";
                return RedirectToAction("Index");
            }

            if (fridge.Quantity < model.QuantityAllocated)
            {
                ModelState.AddModelError("", $"Not enough stock available. Only {fridge.Quantity} left.");
                model.AvailableFridges = _context.Fridge
                                                 .Where(f => f.IsActive && f.Status != "Scrapped" && f.Quantity > 0)
                                                 .ToList();
                return View(model);
            }

            // Create allocation
            var allocation = new FridgeAllocation
            {
                CustomerID = model.CustomerId,
                FridgeId = model.SelectedFridgeID,
                AllocationDate = DateOnly.FromDateTime(DateTime.Now),
                ReturnDate = model.ReturnDate,
                Status = "Allocated",
                QuantityAllocated = model.QuantityAllocated
            };

            _context.FridgeAllocation.Add(allocation);

            // Update fridge stock
            fridge.Quantity -= model.QuantityAllocated;
            fridge.Status = fridge.Quantity > 0 ? "Available" : "Allocated";
            _context.Fridge.Update(fridge);

            _context.SaveChanges();
            // Create initial maintenance request 

            try
            {
                var created = _mrService.CreateInitialRequestForAllocationAsync(fridge.FridgeId)
                                        .GetAwaiter()
                                        .GetResult(); // blocks until async completes
                if (created != null)
                    TempData["Message"] = "Initial maintenance request created for this allocation.";
                else
                    TempData["Message"] = "No new maintenance request created (one already exists).";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Failed to create initial maintenance request: {ex.Message}";
            }
            TempData["SuccessMessage"] = $"Allocated {model.QuantityAllocated} fridge(s) successfully!";
            return RedirectToAction("Details", new { id = model.CustomerId });
            
            
            
        }

        // --------------------------
        // Return Fridge
        // --------------------------
        public async Task<IActionResult> ReturnFridge(int allocationId)
        {
            var allocation = await _context.FridgeAllocation
                                           .Include(a => a.Fridge)
                                           .FirstOrDefaultAsync(a => a.AllocationID == allocationId);

            if (allocation == null || allocation.Status != "Allocated")
                return NotFound();

            allocation.Status = "Returned";
            allocation.ReturnDate = DateOnly.FromDateTime(DateTime.Now);

            if (allocation.Fridge != null)
                allocation.Fridge.Status = "Available";

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Fridge returned successfully!";
            return RedirectToAction(nameof(Details), new { id = allocation.CustomerID });
        }

        // --------------------------
        // Scrap Fridge
        // --------------------------
        public async Task<IActionResult> ScrapFridge(int allocationId)
        {
            var allocation = await _context.FridgeAllocation
                                           .Include(a => a.Fridge)
                                           .FirstOrDefaultAsync(a => a.AllocationID == allocationId);

            if (allocation == null || allocation.Status != "Allocated")
                return NotFound();

            allocation.Status = "Scrapped";
            allocation.ReturnDate = DateOnly.FromDateTime(DateTime.Now);

            if (allocation.Fridge != null)
                allocation.Fridge.Status = "Scrapped";

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Fridge scrapped successfully!";
            return RedirectToAction(nameof(Details), new { id = allocation.CustomerID });
        }

        // --------------------------
        // Customer Details (Allocations)
        // --------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers
                                         .IgnoreQueryFilters()
                                         .Include(c => c.FridgeAllocation)
                                         .ThenInclude(a => a.Fridge)
                                         .FirstOrDefaultAsync(c => c.CustomerID == id);

            if (customer == null) return NotFound();

            var fridgeAllocationsVM = customer.FridgeAllocation?
                .Select(a => new FridgeAllocationViewModel
                {
                    AllocationID = a.AllocationID,
                    FridgeId = a.FridgeId,
                    AllocationDate = a.AllocationDate,
                    ReturnDate = a.ReturnDate,
                    Status = a.Status,
                    QuantityAllocated = a.QuantityAllocated,
                    Fridge = new FridgeViewModel
                    {
                        FridgeId = a.FridgeId,
                        Brand = a.Fridge?.Brand,
                        Model = a.Fridge?.Model,
                        Status = a.Fridge?.Status
                    }
                }).ToList() ?? new List<FridgeAllocationViewModel>();

            var customerVM = new CustomerViewModel
            {
                CustomerId = customer.CustomerID,
                FullNames = customer.FullName,
                LocationId = customer.LocationId,
                PhoneNumber = customer.PhoneNumber,
                IsActive = customer.IsActive,
                RegistrationDate = customer.RegistrationDate,
                FridgeAllocations = fridgeAllocationsVM
            };

            return View(customerVM);
        }

        // --------------------------
        // Search Customers
        // --------------------------
        public async Task<IActionResult> Search(string searchString)
        {
            var query = _context.Customers
                                .IgnoreQueryFilters()
                                .Include(c => c.FridgeAllocation)
                                .ThenInclude(a => a.Fridge)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(c =>
                    c.FullName.ToLower().Contains(searchString) ||
                    c.Email.ToLower().Contains(searchString) ||
                    c.PhoneNumber.ToLower().Contains(searchString));
            }

            var result = await query.ToListAsync();

            var model = result.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerID,
                FullNames = c.FullName,
                LocationId = c.LocationId,
                PhoneNumber = c.PhoneNumber,
                IsActive = c.IsActive,
                RegistrationDate = c.RegistrationDate,
                FridgeAllocations = c.FridgeAllocation
                                     .Select(a => new FridgeAllocationViewModel
                                     {
                                         AllocationID = a.AllocationID,
                                         FridgeId = a.FridgeId,
                                         AllocationDate = a.AllocationDate,
                                         ReturnDate = a.ReturnDate,
                                         Status = a.Status,
                                         QuantityAllocated = a.QuantityAllocated,
                                         Fridge = new FridgeViewModel
                                         {
                                             FridgeId = a.FridgeId,
                                             Brand = a.Fridge?.Brand,
                                             Model = a.Fridge?.Model,
                                             Status = a.Fridge?.Status
                                         }
                                     }).ToList()
            }).ToList();

            return View("Index", model);
        }

        public IActionResult ExportToPdf(int id)
        {
            var customer = _context.Customers
                .Include(c => c.FridgeAllocation)
                .ThenInclude(a => a.Fridge)
                .FirstOrDefault(c => c.CustomerID == id);

            if (customer == null) return NotFound();

            // Set license to Community (free for evaluation/testing)
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            // Generate PDF
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Customer: {customer.FullName}").Bold().FontSize(14);
                        col.Item().Text($"Phone: {customer.PhoneNumber}");
                        col.Item().Text($"Email: {customer.Email}");
                        col.Item().Text($"Address: {customer.LocationId}");
                        col.Item().Text($"Status: {(customer.IsActive ? "Active" : "Inactive")}");
                        col.Item().Text($"Registration Date: {customer.RegistrationDate:yyyy/MM/dd}");
                        col.Item().Text($"Total Fridges Allocated: {customer.FridgeAllocation.Sum(a => a.QuantityAllocated)}");

                        col.Item().Text("\nFridge Allocations:").Bold().FontSize(13);

                        col.Item().Table(table =>
                        {
                            // Use relative widths to avoid layout exceptions
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Brand & Model
                                columns.RelativeColumn(1); // Quantity
                                columns.RelativeColumn(2); // Allocation Date
                                columns.RelativeColumn(2); // Return Date
                                columns.RelativeColumn(1); // Status
                            });

                            // Table header
                            table.Header(header =>
                            {
                                header.Cell().Text("Brand & Model").Bold();
                                header.Cell().Text("Quantity").Bold();
                                header.Cell().Text("Allocation Date").Bold();
                                header.Cell().Text("Return Date").Bold();
                                header.Cell().Text("Status").Bold();
                            });

                            // Table rows
                            foreach (var allocation in customer.FridgeAllocation)
                            {
                                table.Cell().Text($"{allocation.Fridge?.Brand} - {allocation.Fridge?.Model}");
                                table.Cell().Text(allocation.QuantityAllocated.ToString());
                                table.Cell().Text(allocation.AllocationDate.ToString("yyyy/MM/dd"));
                                table.Cell().Text(allocation.ReturnDate?.ToString("yyyy/MM/dd") ?? "N/A");
                                table.Cell().Text(allocation.Status);
                            }
                        });
                    });
                });
            }).GeneratePdf(); // Generate PDF as byte array

            // Return file for download
            return File(pdfBytes, "application/pdf", $"Customer_{customer.CustomerID}_Allocations.pdf");
        }
    }

}

