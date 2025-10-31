using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using FridgeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Rotativa.AspNetCore;
using System.IO;
using Microsoft.Extensions.Logging; // Add this for ILogger

namespace FridgeManagementSystem.Areas.CustomerManagementSubSystem.Controllers
{
    [Area("CustomerManagementSubSystem")]
    public class CustomerLiaisonController : Controller
    {
        private readonly FridgeDbContext _context;
        private readonly IMaintenanceRequestService _mrService;
        private readonly CustomerService _customerService;
        private readonly ILogger<CustomerLiaisonController> _logger; // Add logger

        public CustomerLiaisonController(
            FridgeDbContext context,
            IMaintenanceRequestService mrService,
            CustomerService customerService,
            ILogger<CustomerLiaisonController> logger) // Add logger parameter
        {
            _context = context;
            _mrService = mrService;
            _customerService = customerService;
            _logger = logger; // Initialize logger
        }

        // --------------------------
        // INDEX - Show All Customers (Active + Inactive)
        // --------------------------
        public async Task<IActionResult> Index()
        {
            // ✅ Get all customers including inactive ones
            var customers = await _customerService.GetAllCustomersWithFridgesAsync();

            // ✅ Map to ViewModel (include Active + Inactive)
            var model = customers.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerID,
                FullNames = c.FullName,
                LocationId = c.LocationId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                IsActive = c.IsActive,
                RegistrationDate = c.RegistrationDate,
                FridgeAllocations = c.FridgeAllocation.Select(a => new FridgeAllocationViewModel
                {
                    AllocationID = a.AllocationID,
                    FridgeId = a.FridgeId,
                    QuantityAllocated = a.QuantityAllocated,
                    AllocationDate = a.AllocationDate,
                    ReturnDate = a.ReturnDate, // ✅ keeps 30-day return date logic
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

            // ✅ Optional: show only Active by default
            // comment this line out if you want to show ALL by default
            // model = model.Where(c => c.IsActive).ToList();

            return View(model);
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

            var customer = await _customerService.GetCustomerDetailsAsync(id.Value);
            if (customer == null || !customer.IsActive) return NotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CustomerID)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(CustomerID);
                TempData["SuccessMessage"] = "Customer deactivated successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting customer {CustomerID}");
                TempData["ErrorMessage"] = "Error deactivating customer. Please try again.";
            }

            return RedirectToAction(nameof(Index), new { showInactive = "true" });
        }

        [HttpGet]
        [Route("/CustomerManagementSubSystem/CustomerLiaison/Allocate")]
        public IActionResult AllocateGet(int orderId, int customerId, int orderItemId, int fridgeId = 0)
        {
            if (orderId <= 0 || customerId <= 0 || orderItemId <= 0)
                return NotFound("Missing allocation details.");

            var order = _context.Orders
                .Include(o => o.Customers)
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == orderId && o.CustomerID == customerId);

            if (order == null) return NotFound("Order not found.");

            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.OrderItemId == orderItemId);
            if (orderItem == null) return NotFound("Order item not found.");

            var availableFridges = _context.Fridge
                .Where(f => !_context.FridgeAllocation.Any(a => a.FridgeId == f.FridgeId))
                .ToList();

            // ✅ Load the next maintenance visit for this customer/fridge
            var nextVisit = _context.MaintenanceVisit
                .Include(v => v.Employee)
                .Include(v => v.Fridge)
                .Where(v => v.FridgeId == fridgeId && v.Status == FridgeManagementSystem.Models.TaskStatus.Scheduled)
                .OrderBy(v => v.ScheduledDate)
                .FirstOrDefault();

            ViewBag.Fridges = availableFridges;
            ViewBag.Order = order;
            ViewBag.CustomerId = customerId;
            ViewBag.OrderItemId = orderItemId;
            ViewBag.FridgeId = fridgeId;
            ViewBag.NextVisit = nextVisit; // ✅ Pass to view

            return View();
        }

        // POST: CustomerLiaison/Allocate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/CustomerManagementSubSystem/CustomerLiaison/Allocate")]
        public IActionResult Allocate(int orderId, int customerId, int orderItemId, int fridgeId)
        {
            if (orderId <= 0 || customerId <= 0 || orderItemId <= 0 || fridgeId <= 0)
                return NotFound("Missing allocation details.");

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == orderId && o.CustomerID == customerId);

            if (order == null)
                return NotFound("Order not found.");

            var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (customer == null)
                return NotFound("Customer not found.");

            var fridge = _context.Fridge.FirstOrDefault(f => f.FridgeId == fridgeId);
            if (fridge == null)
                return NotFound("Fridge not found.");

            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.OrderItemId == orderItemId);
            if (orderItem == null)
                return NotFound("Order item not found.");

            // Total already allocated for this order item
            var totalAllocated = _context.FridgeAllocation
                .Where(fa => fa.OrderItemId == orderItemId && fa.CustomerID == customerId)
                .Sum(fa => (int?)fa.QuantityAllocated) ?? 0;

            var remainingToAllocate = orderItem.Quantity - totalAllocated;
            if (remainingToAllocate <= 0)
            {
                TempData["ErrorMessage"] = "All fridges for this item are already allocated.";
                return RedirectToAction("ProcessPendingAllocations");
            }

            // Parse quantity to allocate (defaults to 1)
            int quantityToAllocate = 1;
            if (Request.Form["Quantity"].Count > 0 && int.TryParse(Request.Form["Quantity"], out var q))
                quantityToAllocate = Math.Min(q, remainingToAllocate);

            // Check stock
            if (fridge.Quantity < quantityToAllocate)
            {
                TempData["ErrorMessage"] = $"Not enough stock. Only {fridge.Quantity} fridge(s) available!";
                return RedirectToAction("ProcessPendingAllocations");
            }

            // ✅ Allocation date logic
            var allocationDate = DateOnly.FromDateTime(DateTime.Now);

            // ✅ Create allocation
            var allocation = new FridgeAllocation
            {
                FridgeId = fridge.FridgeId,
                CustomerID = customerId,
                OrderItemId = orderItemId,
                AllocationDate = allocationDate,
                ReturnDate = allocationDate.AddDays(30),
                Status = "Allocated",
                QuantityAllocated = quantityToAllocate
            };

            // ✅ FIX: Actually add to database!
            _context.FridgeAllocation.Add(allocation);

            // ✅ Reduce fridge stock
            fridge.Quantity -= quantityToAllocate;
            fridge.Status = fridge.Quantity == 0 ? "Allocated" : "Available";
            fridge.CustomerID = customerId;

            // ✅ Update order status if fully allocated
            totalAllocated += quantityToAllocate;
            if (totalAllocated >= orderItem.Quantity)
            {
                order.Status = "Fridge Allocated";
                order.OrderProgress = OrderStatus.FridgeAllocated;
            }

            // ✅ Save allocation and updates together
            _context.SaveChanges();

            // ✅ Create maintenance request
            var existingRequest = _context.MaintenanceRequest
                .FirstOrDefault(mr => mr.FridgeId == fridge.FridgeId && mr.IsActive && mr.TaskStatus == Models.TaskStatus.Pending);

            if (existingRequest == null)
            {
                var maintenanceRequest = new MaintenanceRequest
                {
                    FridgeId = fridge.FridgeId,
                    RequestDate = allocation.AllocationDate.HasValue
                        ? allocation.AllocationDate.Value.ToDateTime(TimeOnly.MinValue)
                        : DateTime.Now,
                    TaskStatus = Models.TaskStatus.Pending,
                    IsActive = true
                };
                _context.MaintenanceRequest.Add(maintenanceRequest);
                _context.SaveChanges();

                TempData["Success"] = $"✅ Fridge '{fridge.Brand} {fridge.Model}' allocated ({quantityToAllocate} unit(s)) to {customer.FullName}. " +
                                      $"A maintenance request has been created.";
            }
            else
            {
                TempData["Success"] = $"✅ Fridge '{fridge.Brand} {fridge.Model}' allocated ({quantityToAllocate} unit(s)) to {customer.FullName}. " +
                                      $"Existing maintenance request is still pending.";
            }

            return RedirectToAction("ProcessPendingAllocations");
        }



        private async Task ReloadViewModelData(CustomerAllocationViewModel model)
        {
            _logger.LogInformation("ReloadViewModelData called");

            var paidOrPendingItems = await _context.OrderItems
                .Where(oi => oi.Order.CustomerID == model.CustomerId &&
                             (oi.Order.Status == "Paid" || oi.Order.Status == "Pending"))
                .Include(oi => oi.Fridge)
                .Include(oi => oi.Order)
                .ToListAsync();

            _logger.LogInformation($"Reload - Found {paidOrPendingItems.Count} order items");

            var pendingItems = paidOrPendingItems
                .Where(item => (_context.FridgeAllocation
                                   .Where(fa => fa.CustomerID == model.CustomerId && fa.FridgeId == item.FridgeId)
                                   .Sum(fa => (int?)fa.QuantityAllocated) ?? 0) < item.Quantity)
                .Select(item => new CustomerOrderItemViewModel
                {
                    FridgeId = item.FridgeId,
                    FridgeName = item.Fridge.Brand + " " + item.Fridge.Model,
                    Quantity = item.Quantity,
                    Price = item.Price
                })
                .ToList();

            model.OrderItems = pendingItems;
            _logger.LogInformation($"Reload - Pending items: {pendingItems.Count}");

            model.AvailableFridges = pendingItems
                .Select(p => _context.Fridge
                                     .FirstOrDefault(f => f.FridgeId == p.FridgeId && f.IsActive && f.Quantity > 0))
                .Where(f => f != null)
                .ToList();

            _logger.LogInformation($"Reload - Available fridges: {model.AvailableFridges.Count}");

            var customer = await _customerService.GetCustomerDetailsAsync(model.CustomerId);
            if (customer != null)
                model.CustomerName = customer.FullName;
        }

        // --------------------------
        // Return Fridge (Partial Support)
        // --------------------------
        public async Task<IActionResult> ReturnFridge(int allocationId, int quantityToReturn = 0, DateOnly? returnDate = null)
        {
            var allocation = await _context.FridgeAllocation
                                           .Include(a => a.Fridge)
                                           .FirstOrDefaultAsync(a => a.AllocationID == allocationId);

            if (allocation == null || allocation.Status != "Allocated")
                return NotFound();

            if (quantityToReturn <= 0 || quantityToReturn > allocation.QuantityAllocated)
            {
                TempData["ErrorMessage"] = "Invalid return quantity.";
                return RedirectToAction("Details", "Customer", new { id = allocation.CustomerID });
            }

            // Update return date
            allocation.ReturnDate = returnDate ?? DateOnly.FromDateTime(DateTime.Now);

            // Add returned quantity back to stock
            if (allocation.Fridge != null)
            {
                allocation.Fridge.Quantity += quantityToReturn;

                // If all fridges returned, mark allocation as returned
                if (quantityToReturn == allocation.QuantityAllocated)
                {
                    allocation.Status = "Returned";
                    allocation.Fridge.Status = "Available";
                }
                else
                {
                    // Partial return: reduce the allocated quantity
                    allocation.QuantityAllocated -= quantityToReturn;
                    TempData["SuccessMessage"] = $"{quantityToReturn} fridges returned. {allocation.QuantityAllocated} still allocated.";
                }
            }

            await _context.SaveChangesAsync();

            if (allocation.Status == "Returned")
                TempData["SuccessMessage"] = "All allocated fridges returned successfully and added back to inventory!";

            // Redirect to Inventory stock view
            return RedirectToAction("Index", "InventoryLiaison");
        }


        // --------------------------
        // Scrap Fridge
        // --------------------------
        public async Task<IActionResult> ScrapFridge(int allocationId, DateOnly? scrapDate = null)
        {
            var allocation = await _context.FridgeAllocation
                                           .Include(a => a.Fridge)
                                           .FirstOrDefaultAsync(a => a.AllocationID == allocationId);

            if (allocation == null || allocation.Status != "Allocated")
                return NotFound();

            allocation.Status = "Scrapped";

            // ✅ Use provided date or default to today
            allocation.ReturnDate = scrapDate ?? DateOnly.FromDateTime(DateTime.Now);

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

            var customer = await _customerService.GetCustomerDetailsAsync(id.Value); // ✅ service
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
                Email = customer.Email,
                IsActive = customer.IsActive,
                RegistrationDate = customer.RegistrationDate,
                FridgeAllocations = fridgeAllocationsVM
            };

            return View(customerVM);
        }

        [HttpGet]
        [Route("/CustomerManagementSubSystem/CustomerLiaison/DebugData")]
        public async Task<IActionResult> DebugData()
        {
            var debugInfo = new List<string>();

            // 1. Check customers
            var customers = await _context.Customers.ToListAsync();
            debugInfo.Add($"Total Customers: {customers.Count}");
            foreach (var customer in customers)
            {
                debugInfo.Add($"Customer: {customer.CustomerID} - {customer.FullName} - Active: {customer.IsActive}");
            }

            // 2. Check orders
            var orders = await _context.Orders.Include(o => o.Customers).ToListAsync();
            debugInfo.Add($"\nTotal Orders: {orders.Count}");
            foreach (var order in orders)
            {
                debugInfo.Add($"Order: {order.OrderId} - Customer: {order.CustomerID} - Status: {order.Status}");
            }

            // 3. Check order items
            var orderItems = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Fridge)
                .ToListAsync();

            debugInfo.Add($"\nTotal Order Items: {orderItems.Count}");
            foreach (var item in orderItems)
            {
                debugInfo.Add($"OrderItem: {item.OrderItemId} - Order: {item.OrderId} - Fridge: {item.FridgeId} - Qty: {item.Quantity} - Order Status: {item.Order?.Status}");
            }

            // 4. Check allocations
            var allocations = await _context.FridgeAllocation.ToListAsync();
            debugInfo.Add($"\nTotal Allocations: {allocations.Count}");
            foreach (var alloc in allocations)
            {
                debugInfo.Add($"Allocation: Customer {alloc.CustomerID} - Fridge {alloc.FridgeId} - Qty: {alloc.QuantityAllocated}");
            }

            return Content(string.Join("\n", debugInfo));
        }

        [HttpGet]
        [Route("/CustomerManagementSubSystem/CustomerLiaison/ProcessPendingAllocations")]
        public async Task<IActionResult> ProcessPendingAllocations()
        {
            // Include Paid and partially/fully allocated orders
            var paidOrders = await _context.Orders
                .Where(o => o.Status == "Paid" || o.Status == "Fridge Allocated")
                .Include(o => o.Customers)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Fridge)
                .ToListAsync();

            var viewModel = new List<PendingAllocationViewModel>();

            foreach (var order in paidOrders)
            {
                foreach (var item in order.OrderItems)
                {
                    var totalAllocated = await _context.FridgeAllocation
                        .Where(fa => fa.OrderItemId == item.OrderItemId && fa.CustomerID == order.CustomerID)
                        .SumAsync(fa => (int?)fa.QuantityAllocated) ?? 0;

                    var remaining = item.Quantity - totalAllocated;

                    // Always add to view model, status changes based on remaining
                    viewModel.Add(new PendingAllocationViewModel
                    {
                        OrderId = order.OrderId,
                        OrderItemId = item.OrderItemId,
                        CustomerId = order.CustomerID,
                        CustomerName = order.Customers?.FullName ?? "Unknown",
                        FridgeId = item.FridgeId,
                        FridgeBrand = item.Fridge?.Brand ?? "N/A",
                        FridgeModel = item.Fridge?.Model ?? "N/A",
                        QuantityOrdered = item.Quantity,
                        QuantityAllocated = totalAllocated,
                        QuantityPending = remaining,
                        Status = remaining > 0 ? "Pending" : "Allocated" // <-- status updates
                    });
                }
            }

            if (!viewModel.Any())
                TempData["InfoMessage"] = "No pending allocations found.";

            return View(viewModel);
        }


        // --------------------------
        // Search Customers (Supports Soft Delete)
        // --------------------------
        public async Task<IActionResult> Search(string searchString)
        {
            // ✅ Always get both Active + Inactive customers
            var customers = await _customerService.GetAllCustomersWithFridgesAsync();

            // ✅ If user searched something
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                customers = customers
                    .Where(c =>
                        (!string.IsNullOrEmpty(c.FullName) && c.FullName.ToLower().Contains(searchString)) ||
                        (!string.IsNullOrEmpty(c.Email) && c.Email.ToLower().Contains(searchString)) ||
                        (!string.IsNullOrEmpty(c.PhoneNumber) && c.PhoneNumber.ToLower().Contains(searchString)))
                    .ToList();
            }

            // ✅ Map to ViewModel (show both Active & Inactive)
            var model = customers.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerID,
                FullNames = c.FullName,
                LocationId = c.LocationId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                IsActive = c.IsActive,
                RegistrationDate = c.RegistrationDate,
                FridgeAllocations = c.FridgeAllocation.Select(a => new FridgeAllocationViewModel
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

            // ✅ Keep search term in textbox after searching
            ViewData["CurrentFilter"] = searchString;

            return View("Index", model);
        }

        public IActionResult ExportToPdf(int id)
        {
            var customer = _context.Customers
                .Include(c => c.FridgeAllocation)
                .ThenInclude(a => a.Fridge)
                .FirstOrDefault(c => c.CustomerID == id);

            if (customer == null) return NotFound();

            QuestPDF.Settings.License = LicenseType.Community;

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
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Brand & Model").Bold();
                                header.Cell().Text("Quantity").Bold();
                                header.Cell().Text("Allocation Date").Bold();
                                header.Cell().Text("Return Date").Bold();
                                header.Cell().Text("Status").Bold();
                            });

                            foreach (var allocation in customer.FridgeAllocation)
                            {
                                table.Cell().Text($"{allocation.Fridge?.Brand} - {allocation.Fridge?.Model}");
                                table.Cell().Text(allocation.QuantityAllocated.ToString());

                                // ✅ Safely format AllocationDate
                                table.Cell().Text(allocation.AllocationDate.HasValue
                                    ? allocation.AllocationDate.Value.ToString("yyyy/MM/dd")
                                    : "N/A");

                                // ✅ Safely format ReturnDate
                                table.Cell().Text(allocation.ReturnDate.HasValue
                                    ? allocation.ReturnDate.Value.ToString("yyyy/MM/dd")
                                    : "N/A");

                                table.Cell().Text(allocation.Status);
                            }
                        });
                    });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Customer_{customer.CustomerID}_Allocations.pdf");
        }


    }
}
