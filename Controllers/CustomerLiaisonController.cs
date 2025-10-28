using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using FridgeManagementSystem.ViewModels;
using System.IO;
using Rotativa.AspNetCore;
using FridgeManagementSystem.Data;

namespace FridgeManagementSystem.Controllers
{
    public class CustomerLiaisonController : Controller
    {
        private readonly FridgeDbContext _context;
        private readonly CustomerService _customerService; // add service

        public CustomerLiaisonController(FridgeDbContext context, CustomerService customerService)
        {
            _context = context;
            _customerService = customerService; //initialize service
        }

        // --------------------------
        // List Customers (Active + Inactive)
        // --------------------------
        public async Task<IActionResult> Index()
        {

            ViewData["Sidebar"] = "Customer";
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
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            customer.IsActive = true;
            customer.RegistrationDate = DateOnly.FromDateTime(DateTime.Now);
            _context.Add(customer);
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
        // GET: Allocate fridge to customer
        public async Task<IActionResult> Allocate(int customerId)
        {
            var customer = await _customerService.GetCustomerDetailsAsync(customerId);
            if (customer == null) return NotFound();

            // ✅ ONLY get fridges that this customer has PAID for
            var paidOrderItems = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Fridge)
                .Where(oi => oi.Order.CustomerID == customerId &&
                             oi.Order.Status == "Paid" &&
                             oi.Fridge.IsActive)
                .ToListAsync();

            // ✅ Calculate remaining quantities to allocate
            var allocationViewModels = new List<CustomerOrderItemViewModel>();

            foreach (var orderItem in paidOrderItems)
            {
                var totalAllocatedSoFar = await _context.FridgeAllocation
                    .Where(fa => fa.CustomerID == customerId && fa.FridgeId == orderItem.FridgeId)
                    .SumAsync(fa => (int?)fa.QuantityAllocated) ?? 0;

                var remainingToAllocate = orderItem.Quantity - totalAllocatedSoFar;

                if (remainingToAllocate > 0)
                {
                    allocationViewModels.Add(new CustomerOrderItemViewModel
                    {
                        OrderItemId = orderItem.OrderItemId,
                        FridgeId = orderItem.FridgeId,
                        FridgeBrand = orderItem.Fridge.Brand,
                        FridgeModel = orderItem.Fridge.Model,
                        OrderedQuantity = orderItem.Quantity,
                        AlreadyAllocated = totalAllocatedSoFar,
                        RemainingToAllocate = remainingToAllocate,
                        UnitPrice = orderItem.Price,
                        FridgeStock = orderItem.Fridge.Quantity
                    });
                }
            }

            if (!allocationViewModels.Any())
            {
                TempData["ErrorMessage"] = "No pending allocations found. Customer may not have any paid orders or all items are already allocated.";
                return RedirectToAction("Details", new { id = customerId });
            }

            var model = new CustomerAllocationViewModel
            {
                CustomerId = customer.CustomerID,
                CustomerName = customer.FullName,
                OrderItems = allocationViewModels
            };

            return View(model);
        }

        //==================
        //POST ALLOCATE
        //==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Allocate(CustomerAllocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload order items if validation fails
                await ReloadOrderItems(model);
                return View(model);
            }

            // Process each order item that has allocation quantity
            foreach (var orderItem in model.OrderItems.Where(oi => oi.QuantityToAllocate > 0))
            {
                // Validate the order item belongs to this customer and is paid
                var validOrderItem = await _context.OrderItems
                    .Include(oi => oi.Order)
                    .Include(oi => oi.Fridge)
                    .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItem.OrderItemId &&
                                              oi.Order.CustomerID == model.CustomerId &&
                                              oi.Order.Status == "Paid");

                if (validOrderItem == null)
                {
                    ModelState.AddModelError("", $"Invalid order item for allocation.");
                    await ReloadOrderItems(model);
                    return View(model);
                }

                // Check remaining allocation limit
                var totalAllocated = await _context.FridgeAllocation
                    .Where(fa => fa.CustomerID == model.CustomerId && fa.FridgeId == orderItem.FridgeId)
                    .SumAsync(fa => (int?)fa.QuantityAllocated) ?? 0;

                var remainingToAllocate = validOrderItem.Quantity - totalAllocated;

                if (orderItem.QuantityToAllocate > remainingToAllocate)
                {
                    ModelState.AddModelError("", $"Cannot allocate more than {remainingToAllocate} units of {orderItem.FridgeBrand} {orderItem.FridgeModel}.");
                    await ReloadOrderItems(model);
                    return View(model);
                }

                // Check fridge stock
                if (orderItem.QuantityToAllocate > validOrderItem.Fridge.Quantity)
                {
                    ModelState.AddModelError("", $"Insufficient stock for {orderItem.FridgeBrand} {orderItem.FridgeModel}. Available: {validOrderItem.Fridge.Quantity}");
                    await ReloadOrderItems(model);
                    return View(model);
                }

                // ✅ PERFORM ALLOCATION
                var allocation = new FridgeAllocation
                {
                    CustomerID = model.CustomerId,
                    FridgeId = orderItem.FridgeId,
                    AllocationDate = DateOnly.FromDateTime(DateTime.Now),
                    ReturnDate = orderItem.ReturnDate,
                    Status = "Allocated",
                    QuantityAllocated = orderItem.QuantityToAllocate
                };
                _context.FridgeAllocation.Add(allocation);

                // Update fridge stock
                validOrderItem.Fridge.Quantity -= orderItem.QuantityToAllocate;
                if (validOrderItem.Fridge.Quantity == 0)
                {
                    validOrderItem.Fridge.Status = "Allocated";
                }
                _context.Fridge.Update(validOrderItem.Fridge);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Fridge(s) allocated successfully!";
            return RedirectToAction("Details", new { id = model.CustomerId });
        }

        private async Task ReloadOrderItems(CustomerAllocationViewModel model)
        {
            var paidOrderItems = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Fridge)
                .Where(oi => oi.Order.CustomerID == model.CustomerId && oi.Order.Status == "Paid")
                .ToListAsync();

            var orderItemViewModels = new List<CustomerOrderItemViewModel>();

            foreach (var orderItem in paidOrderItems)
            {
                var totalAllocated = await _context.FridgeAllocation
                    .Where(fa => fa.CustomerID == model.CustomerId && fa.FridgeId == orderItem.FridgeId)
                    .SumAsync(fa => (int?)fa.QuantityAllocated) ?? 0;

                var remainingToAllocate = orderItem.Quantity - totalAllocated;

                if (remainingToAllocate > 0)
                {
                    orderItemViewModels.Add(new CustomerOrderItemViewModel
                    {
                        OrderItemId = orderItem.OrderItemId,
                        FridgeId = orderItem.FridgeId,
                        FridgeBrand = orderItem.Fridge.Brand,
                        FridgeModel = orderItem.Fridge.Model,
                        OrderedQuantity = orderItem.Quantity,
                        AlreadyAllocated = totalAllocated,
                        RemainingToAllocate = remainingToAllocate,
                        UnitPrice = orderItem.Price,
                        FridgeStock = orderItem.Fridge.Quantity
                    });
                }
            }

            model.OrderItems = orderItemViewModels;
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

        // ==========================
        // PROCESS PENDING ALLOCATIONS
        // ==========================
        public async Task<IActionResult> ProcessPendingAllocations()
        {
            // Get all customers with pending or paid orders that are not fully allocated
            var customersWithPending = await _context.Customers
                .Select(c => new
                {
                    Customer = c,
                    PendingItems = _context.OrderItems
                        .Include(oi => oi.Order)
                        .Include(oi => oi.Fridge)
                        .Where(oi => oi.Order.CustomerID == c.CustomerID &&
                                     (oi.Order.Status == "Paid" || oi.Order.Status == "Pending"))
                        .ToList()
                })
                .ToListAsync();

            // Filter customers who actually have pending items left to allocate
            var customerPendingList = new List<PendingAllocationViewModel>();

            foreach (var c in customersWithPending)
            {
                var pendingItems = c.PendingItems
                    .Where(item =>
                        (_context.FridgeAllocation
                            .Where(fa => fa.CustomerID == c.Customer.CustomerID && fa.FridgeId == item.FridgeId)
                            .Sum(fa => (int?)fa.QuantityAllocated) ?? 0) < item.Quantity
                    )
                    .ToList();

                if (pendingItems.Any())
                {
                    customerPendingList.Add(new PendingAllocationViewModel
                    {
                        CustomerId = c.Customer.CustomerID,
                        CustomerName = c.Customer.FullName,
                        FridgeId = pendingItems.Count
                    });
                }
            }

            return View(customerPendingList);
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
                    c.LocationId.ToString().Contains(searchString) ||
                    c.PhoneNumber.ToLower().Contains(searchString));
            }

            var result = await query.ToListAsync();

            var model = result.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerID,
                FullNames = c.FullName,
                LocationId = c.LocationId,
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
                        col.Item().Text($"Email: {customer.LocationId}");
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
                                table.Cell().Text(allocation.AllocationDate.HasValue
                                 ? allocation.AllocationDate.Value.ToString("yyyy/MM/dd")
                                 : "");
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

