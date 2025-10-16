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

namespace CustomerManagementSubSystem.Controllers
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
        // List Customers (Active + Inactive)
        // --------------------------
        public async Task<IActionResult> Index()
        {
            // ✅ use service instead of _context directly
            var customers = await _customerService.GetAllCustomersWithFridgesAsync();

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

        //// GET: Allocate fridge to customer
        //public async Task<IActionResult> Allocate(int customerId)
        //{
        //    _logger.LogInformation("=== ALLOCATE GET STARTED ===");
        //    _logger.LogInformation($"Received customerId: {customerId}");

        //    if (customerId == 0)
        //    {
        //        _logger.LogWarning("CustomerId is 0 - redirecting to Index");
        //        TempData["ErrorMessage"] = "Customer ID is missing!";
        //        return RedirectToAction("Index");
        //    }

        //    var customer = await _customerService.GetCustomerDetailsAsync(customerId);
        //    if (customer == null)
        //    {
        //        _logger.LogWarning($"Customer not found with ID: {customerId}");
        //        return NotFound();
        //    }

        //    _logger.LogInformation($"Customer found: {customer.FullName} (ID: {customer.CustomerID})");

        //    // Get customer's orders with status Paid or Pending
        //    var paidOrPendingItems = await _context.OrderItems
        //        .Where(oi => oi.Order.CustomerID == customerId &&
        //                     (oi.Order.Status == "Paid" || oi.Order.Status == "Pending"))
        //        .Include(oi => oi.Fridge)
        //        .Include(oi => oi.Order) // Make sure Order is included
        //        .ToListAsync();

        //    _logger.LogInformation($"Found {paidOrPendingItems.Count} order items for customer");

        //    // Filter out fully allocated items
        //    var pendingItems = new List<CustomerOrderItemViewModel>();
        //    foreach (var item in paidOrPendingItems)
        //    {
        //        _logger.LogInformation($"Processing order item: Fridge {item.FridgeId}, Quantity {item.Quantity}, Order Status: {item.Order?.Status}");

        //        var totalAllocated = await _context.FridgeAllocation
        //            .Where(fa => fa.CustomerID == customerId && fa.FridgeId == item.FridgeId)
        //            .SumAsync(fa => (int?)fa.QuantityAllocated) ?? 0;

        //        _logger.LogInformation($"Total already allocated: {totalAllocated}");

        //        if (totalAllocated < item.Quantity)
        //        {
        //            pendingItems.Add(new CustomerOrderItemViewModel
        //            {
        //                FridgeId = item.FridgeId,
        //                FridgeName = item.Fridge.Brand + " " + item.Fridge.Model,
        //                Quantity = item.Quantity,
        //                Price = item.Price
        //            });
        //            _logger.LogInformation($"✅ Added to pending items: {item.Fridge.Brand} {item.Fridge.Model}");
        //        }
        //        else
        //        {
        //            _logger.LogInformation($"❌ Skipped - already fully allocated");
        //        }
        //    }

        //    var model = new CustomerAllocationViewModel
        //    {
        //        CustomerId = customer.CustomerID,
        //        CustomerName = customer.FullName,
        //        Status = "Pending",
        //        OrderItems = pendingItems,
        //        AvailableFridges = pendingItems.Select(p => _context.Fridge
        //                                            .FirstOrDefault(f => f.FridgeId == p.FridgeId && f.IsActive && f.Quantity > 0))
        //                                      .Where(f => f != null)
        //                                      .ToList()
        //    };

        //    _logger.LogInformation($"Final model - AvailableFridges count: {model.AvailableFridges.Count}");
        //    _logger.LogInformation($"Final model - OrderItems count: {model.OrderItems.Count}");

        //    if (!model.AvailableFridges.Any())
        //    {
        //        _logger.LogWarning("No available fridges - redirecting to Index");
        //        TempData["ErrorMessage"] = "No fridges available for allocation.";
        //        return RedirectToAction("Index");
        //    }

        //    _logger.LogInformation("=== ALLOCATE GET COMPLETED - RETURNING VIEW ===");
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Allocate(CustomerAllocationViewModel model)
        //{
        //    _logger.LogInformation("=== ALLOCATE POST STARTED ===");
        //    _logger.LogInformation($"Model received - CustomerId: {model.CustomerId}, SelectedFridgeID: {model.SelectedFridgeID}, QuantityAllocated: {model.QuantityAllocated}");

        //    // TEMPORARY: Bypass validation for testing
        //    if (model.SelectedFridgeID == 0)
        //    {
        //        ModelState.AddModelError("SelectedFridgeID", "Please select a fridge");
        //    }
        //    if (model.QuantityAllocated <= 0)
        //    {
        //        ModelState.AddModelError("QuantityAllocated", "Quantity must be greater than 0");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        _logger.LogWarning("Model validation failed");
        //        await ReloadViewModelData(model);
        //        return View(model);
        //    }

        //    try
        //    {
        //        // Your existing allocation logic here...
        //        var fridge = await _context.Fridge.FindAsync(model.SelectedFridgeID);
        //        if (fridge == null)
        //        {
        //            ModelState.AddModelError("", "Fridge not found");
        //            await ReloadViewModelData(model);
        //            return View(model);
        //        }

        //        _logger.LogInformation($"Allocating {model.QuantityAllocated} fridges to customer {model.CustomerId}");

        //        // Perform allocation (simplified for testing)
        //        var allocation = new FridgeAllocation
        //        {
        //            CustomerID = model.CustomerId,
        //            FridgeId = model.SelectedFridgeID,
        //            AllocationDate = DateOnly.FromDateTime(DateTime.Now),
        //            ReturnDate = model.ReturnDate ?? DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        //            Status = "Allocated",
        //            QuantityAllocated = model.QuantityAllocated
        //        };
        //        _context.FridgeAllocation.Add(allocation);

        //        await _context.SaveChangesAsync();

        //        TempData["SuccessMessage"] = $"Successfully allocated {model.QuantityAllocated} fridge(s)!";
        //        return RedirectToAction(nameof(Allocate), new { customerId = model.CustomerId });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error during allocation");
        //        ModelState.AddModelError("", $"Error: {ex.Message}");
        //        await ReloadViewModelData(model);
        //        return View(model);
        //    }
        //}

        // GET: CustomerLiaison/Allocate
        public IActionResult Allocate(int orderId, int customerId)
        {
            var order = _context.Orders
                .Include(o => o.Customers)
                .FirstOrDefault(o => o.OrderId == orderId && o.CustomerID == customerId);

            if (order == null)
                return NotFound();

            // Get only available fridges (not yet allocated)
            var availableFridges = _context.Fridge
                .Where(f => !_context.FridgeAllocation.Any(a => a.FridgeId == f.FridgeId))
                .ToList();

            ViewBag.Fridges = availableFridges;
            ViewBag.Order = order;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Allocate(int orderId, int customerId, int fridgeId)
        {
            var order = _context.Orders
                .Include(o => o.Customers)
                .FirstOrDefault(o => o.OrderId == orderId && o.CustomerID == customerId);

            if (order == null)
                return NotFound();

            var fridge = _context.Fridge.FirstOrDefault(f => f.FridgeId == fridgeId);
            if (fridge == null)
                return NotFound();

            var allocation = new FridgeAllocation
            {
                FridgeId = fridge.FridgeId,
                CustomerID = customerId,
                AllocationDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.FridgeAllocation.Add(allocation);

            // Update the order status
            order.Status = "Fridge Allocated";
            order.OrderProgress = OrderStatus.FridgeAllocated;

            _context.SaveChanges();

            TempData["Success"] = $"Fridge {fridge.FridgeType} successfully allocated to {order.Customers.FullName}.";

            return RedirectToAction("Index");
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
            _logger.LogInformation("ProcessPendingAllocations called");

            try
            {
                // Get all PAID orders with their items and customer info
                var paidOrders = await _context.Orders
                    .Where(o => o.Status == "Paid")
                    .Include(o => o.Customers)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Fridge)
                    .ToListAsync();

                _logger.LogInformation($"Found {paidOrders.Count} paid orders");

                var viewModel = new List<PendingAllocationViewModel>();

                foreach (var order in paidOrders)
                {
                    foreach (var orderItem in order.OrderItems)
                    {
                        var customerId = order.CustomerID;
                        var customerName = order.Customers?.FullName ?? "Unknown";

                        // Calculate total allocated for this customer + fridge
                        var totalAllocated = await _context.FridgeAllocation
                            .Where(fa => fa.CustomerID == customerId && fa.FridgeId == orderItem.FridgeId)
                            .SumAsync(fa => (int?)fa.QuantityAllocated) ?? 0;

                        var remainingToAllocate = orderItem.Quantity - totalAllocated;

                        _logger.LogInformation($"Order {order.OrderId}, Item {orderItem.OrderItemId}: Customer {customerId}, Fridge {orderItem.FridgeId}, Ordered: {orderItem.Quantity}, Allocated: {totalAllocated}, Remaining: {remainingToAllocate}");

                        // Only include items that still need allocation
                        if (remainingToAllocate > 0)
                        {
                            var vm = new PendingAllocationViewModel
                            {
                                OrderItemId = orderItem.OrderItemId,
                                CustomerId = customerId,
                                CustomerName = customerName,
                                FridgeId = orderItem.FridgeId,
                                FridgeBrand = orderItem.Fridge?.Brand ?? "N/A",
                                FridgeModel = orderItem.Fridge?.Model ?? "N/A",
                                QuantityOrdered = orderItem.Quantity,
                                QuantityAllocated = totalAllocated,
                                QuantityPending = remainingToAllocate,
                                Status = "Pending"
                            };

                            viewModel.Add(vm);
                        }
                    }
                }

                _logger.LogInformation($"Final view model has {viewModel.Count} pending allocations");

                if (!viewModel.Any())
                {
                    TempData["InfoMessage"] = "No pending allocations found. All paid orders have been fully allocated.";
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProcessPendingAllocations");
                TempData["ErrorMessage"] = "An error occurred while loading pending allocations.";
                return View(new List<PendingAllocationViewModel>());
            }
        }

        // --------------------------
        // Search Customers
        // --------------------------
        public async Task<IActionResult> Search(string searchString)
        {
            var customers = await _customerService.GetAllCustomersWithFridgesAsync(); // ✅ service

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                customers = customers
                    .Where(c =>
                        c.FullName.ToLower().Contains(searchString) ||
                        c.Email.ToLower().Contains(searchString) ||
                        c.PhoneNumber.ToLower().Contains(searchString))
                    .ToList();
            }

            var model = customers.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerID,
                FullNames = c.FullName,
                LocationId = c.LocationId,
                PhoneNumber = c.PhoneNumber,
                IsActive = c.IsActive,
                RegistrationDate = c.RegistrationDate,
                FridgeAllocations = c.FridgeAllocation.Select(a => new FridgeAllocationViewModel
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
                                table.Cell().Text(allocation.AllocationDate.ToString("yyyy/MM/dd"));
                                table.Cell().Text(allocation.ReturnDate?.ToString("yyyy/MM/dd") ?? "N/A");
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


//using FridgeManagementSystem.Data;
//using FridgeManagementSystem.Models;
//using FridgeManagementSystem.Services;
//using FridgeManagementSystem.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using QuestPDF.Fluent;
//using QuestPDF.Helpers;
//using QuestPDF.Infrastructure;
//using Rotativa.AspNetCore;
//using System.IO;

//namespace CustomerManagementSubSystem.Controllers
//{
//    [Area("CustomerManagementSubSystem")]
//    public class CustomerLiaisonController : Controller
//    {
//        private readonly FridgeDbContext _context;
//        private readonly IMaintenanceRequestService _mrService;
//        private readonly CustomerService _customerService;   // ✅ use shared service

//        public CustomerLiaisonController(
//            FridgeDbContext context,
//            IMaintenanceRequestService mrService,
//            CustomerService customerService)
//        {
//            _context = context;
//            _mrService = mrService;
//            _customerService = customerService;
//        }

//        // --------------------------
//        // List Customers (Active + Inactive)
//        // --------------------------
//        public async Task<IActionResult> Index()
//        {
//            // ✅ use service instead of _context directly
//            var customers = await _customerService.GetAllCustomersWithFridgesAsync();

//            var model = customers.Select(c => new CustomerViewModel
//            {
//                CustomerId = c.CustomerID,
//                FullNames = c.FullName,
//                LocationId = c.LocationId,
//                PhoneNumber = c.PhoneNumber,
//                Email = c.Email,
//                IsActive = c.IsActive,
//                RegistrationDate = c.RegistrationDate,
//                FridgeAllocations = c.FridgeAllocation.Select(a => new FridgeAllocationViewModel
//                {
//                    AllocationID = a.AllocationID,
//                    FridgeId = a.FridgeId,
//                    QuantityAllocated = a.QuantityAllocated,
//                    AllocationDate = a.AllocationDate,
//                    ReturnDate = a.ReturnDate,
//                    Status = a.Status,
//                    Fridge = new FridgeViewModel
//                    {
//                        FridgeId = a.FridgeId,
//                        Brand = a.Fridge?.Brand,
//                        Model = a.Fridge?.Model,
//                        Status = a.Fridge?.Status
//                    }
//                }).ToList()
//            }).ToList();

//            return View(model);
//        }

//        // --------------------------
//        // Create Customer
//        // --------------------------
//        public IActionResult Create()
//        {
//            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "LocationName");
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Customer customer)
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "LocationName");
//                return View(customer);
//            }

//            customer.IsActive = true;
//            customer.RegistrationDate = DateOnly.FromDateTime(DateTime.Now);

//            _context.Customers.Add(customer);
//            await _context.SaveChangesAsync();

//            TempData["SuccessMessage"] = "Customer created successfully!";
//            return RedirectToAction(nameof(Index));
//        }

//        // --------------------------
//        // Edit Customer
//        // --------------------------
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null) return NotFound();

//            var customer = await _context.Customers.FindAsync(id);
//            if (customer == null || !customer.IsActive) return NotFound();

//            return View(customer);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(Customer customer)
//        {
//            if (!ModelState.IsValid)
//                return View(customer);

//            try
//            {
//                _context.Update(customer);
//                await _context.SaveChangesAsync();
//                TempData["SuccessMessage"] = "Customer updated successfully!";
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!_context.Customers.Any(e => e.CustomerID == customer.CustomerID))
//                    return NotFound();
//                else throw;
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        // --------------------------
//        // Delete Customer (Soft Delete)
//        // --------------------------
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null) return NotFound();

//            var customer = await _context.Customers
//                                         .FirstOrDefaultAsync(c => c.CustomerID == id && c.IsActive);

//            if (customer == null) return NotFound();

//            return View(customer);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int CustomerID)
//        {
//            var customer = await _context.Customers.FindAsync(CustomerID);
//            if (customer != null)
//            {
//                customer.IsActive = false;
//                await _context.SaveChangesAsync();
//                TempData["SuccessMessage"] = "Customer deleted successfully!";
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        // GET: Allocate fridge to customer
//        public async Task<IActionResult> Allocate(int customerId)
//        {
//            var customer = await _customerService.GetCustomerDetailsAsync(customerId);
//            if (customer == null) return NotFound();

//            // Get customer's orders with status Paid or Pending
//            var paidOrPendingItems = await _context.OrderItems
//                .Where(oi => oi.Order.CustomerID == customerId &&
//                             (oi.Order.Status == "Paid" || oi.Order.Status == "Pending"))
//                .Include(oi => oi.Fridge)
//                .ToListAsync();

//            // Filter out fully allocated items
//            var pendingItems = new List<CustomerOrderItemViewModel>();
//            foreach (var item in paidOrPendingItems)
//            {
//                var totalAllocated = await _context.FridgeAllocation
//                    .Where(fa => fa.CustomerID == customerId && fa.FridgeId == item.FridgeId)
//                    .SumAsync(fa => (int?)fa.QuantityAllocated) ?? 0;

//                if (totalAllocated < item.Quantity)
//                {
//                    pendingItems.Add(new CustomerOrderItemViewModel
//                    {
//                        FridgeId = item.FridgeId,
//                        FridgeName = item.Fridge.Brand + " " + item.Fridge.Model,
//                        Quantity = item.Quantity,
//                        Price = item.Price
//                    });
//                }
//            }

//            var model = new CustomerAllocationViewModel
//            {
//                CustomerId = customer.CustomerID,
//                CustomerName = customer.FullName,
//                Status = "Pending",
//                OrderItems = pendingItems,
//                AvailableFridges = pendingItems.Select(p => _context.Fridge
//                                                    .FirstOrDefault(f => f.FridgeId == p.FridgeId && f.IsActive && f.Quantity > 0))
//                                              .Where(f => f != null)
//                                              .ToList()
//            };

//            if (!model.AvailableFridges.Any())
//            {
//                TempData["ErrorMessage"] = "No fridges available for allocation.";
//                return RedirectToAction("Index");
//            }

//            return View(model);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Allocate(CustomerAllocationViewModel model)
//        {
//            try
//            {
//                // Always reload available fridges for dropdown
//                await ReloadViewModelData(model);

//                if (!ModelState.IsValid)
//                    return View(model);

//                // Validate fridge selection
//                var fridge = await _context.Fridge.FindAsync(model.SelectedFridgeID);
//                if (fridge == null || !fridge.IsActive || fridge.Quantity < model.QuantityAllocated)
//                {
//                    ModelState.AddModelError("", "Selected fridge is not available or insufficient quantity.");
//                    return View(model);
//                }

//                // Validate customer has Paid or Pending order for this fridge
//                var orderItem = await _context.OrderItems
//                    .Include(oi => oi.Order)
//                    .FirstOrDefaultAsync(oi => oi.Order.CustomerID == model.CustomerId &&
//                                               oi.FridgeId == model.SelectedFridgeID &&
//                                               (oi.Order.Status == "Paid" || oi.Order.Status == "Pending"));

//                if (orderItem == null)
//                {
//                    TempData["ErrorMessage"] = "Customer has no order for this fridge.";
//                    return RedirectToAction("Allocate", new { customerId = model.CustomerId });
//                }

//                // Check remaining allocation
//                var totalAllocated = await _context.FridgeAllocation
//                    .Where(fa => fa.CustomerID == model.CustomerId && fa.FridgeId == model.SelectedFridgeID)
//                    .SumAsync(fa => (int?)fa.QuantityAllocated) ?? 0;

//                var remainingToAllocate = orderItem.Quantity - totalAllocated;
//                if (model.QuantityAllocated > remainingToAllocate)
//                {
//                    ModelState.AddModelError("", $"Cannot allocate more than {remainingToAllocate} fridge(s).");
//                    return View(model);
//                }

//                // Perform allocation
//                var allocation = new FridgeAllocation
//                {
//                    CustomerID = model.CustomerId,
//                    FridgeId = model.SelectedFridgeID,
//                    AllocationDate = DateOnly.FromDateTime(DateTime.Now),
//                    ReturnDate = model.ReturnDate ?? DateOnly.FromDateTime(DateTime.Now),
//                    Status = "Allocated",
//                    QuantityAllocated = model.QuantityAllocated
//                };
//                _context.FridgeAllocation.Add(allocation);

//                fridge.Quantity -= model.QuantityAllocated;
//                fridge.Status = fridge.Quantity == 0 ? "Allocated" : "Available";
//                _context.Fridge.Update(fridge);

//                await _context.SaveChangesAsync();

//                TempData["SuccessMessage"] = $"Allocated {model.QuantityAllocated} fridge(s) successfully!";
//                return RedirectToAction(nameof(Allocate), new { customerId = model.CustomerId });
//            }
//            catch (Exception ex)
//            {
//                ModelState.AddModelError("", $"Error during allocation: {ex.Message}");
//                await ReloadViewModelData(model);
//                return View(model);
//            }
//        }

//        private async Task ReloadViewModelData(CustomerAllocationViewModel model)
//        {
//            var paidOrPendingItems = await _context.OrderItems
//                .Where(oi => oi.Order.CustomerID == model.CustomerId &&
//                             (oi.Order.Status == "Paid" || oi.Order.Status == "Pending"))
//                .Include(oi => oi.Fridge)
//                .ToListAsync();

//            var pendingItems = paidOrPendingItems
//                .Where(item => (_context.FridgeAllocation
//                                   .Where(fa => fa.CustomerID == model.CustomerId && fa.FridgeId == item.FridgeId)
//                                   .Sum(fa => (int?)fa.QuantityAllocated) ?? 0) < item.Quantity)
//                .Select(item => new CustomerOrderItemViewModel
//                {
//                    FridgeId = item.FridgeId,
//                    FridgeName = item.Fridge.Brand + " " + item.Fridge.Model,
//                    Quantity = item.Quantity,
//                    Price = item.Price
//                })
//                .ToList();

//            model.OrderItems = pendingItems;

//            model.AvailableFridges = pendingItems
//                .Select(p => _context.Fridge
//                                     .FirstOrDefault(f => f.FridgeId == p.FridgeId && f.IsActive && f.Quantity > 0))
//                .Where(f => f != null)
//                .ToList();

//            var customer = await _customerService.GetCustomerDetailsAsync(model.CustomerId);
//            if (customer != null)
//                model.CustomerName = customer.FullName;
//        }

//        // --------------------------
//        // Return Fridge
//        // --------------------------
//        public async Task<IActionResult> ReturnFridge(int allocationId)
//        {
//            var allocation = await _context.FridgeAllocation
//                                           .Include(a => a.Fridge)
//                                           .FirstOrDefaultAsync(a => a.AllocationID == allocationId);

//            if (allocation == null || allocation.Status != "Allocated")
//                return NotFound();

//            allocation.Status = "Returned";
//            allocation.ReturnDate = DateOnly.FromDateTime(DateTime.Now);

//            if (allocation.Fridge != null)
//                allocation.Fridge.Status = "Available";

//            await _context.SaveChangesAsync();
//            TempData["SuccessMessage"] = "Fridge returned successfully!";
//            return RedirectToAction(nameof(Details), new { id = allocation.CustomerID });
//        }

//        // --------------------------
//        // Scrap Fridge
//        // --------------------------
//        public async Task<IActionResult> ScrapFridge(int allocationId)
//        {
//            var allocation = await _context.FridgeAllocation
//                                           .Include(a => a.Fridge)
//                                           .FirstOrDefaultAsync(a => a.AllocationID == allocationId);

//            if (allocation == null || allocation.Status != "Allocated")
//                return NotFound();

//            allocation.Status = "Scrapped";
//            allocation.ReturnDate = DateOnly.FromDateTime(DateTime.Now);

//            if (allocation.Fridge != null)
//                allocation.Fridge.Status = "Scrapped";

//            await _context.SaveChangesAsync();
//            TempData["SuccessMessage"] = "Fridge scrapped successfully!";
//            return RedirectToAction(nameof(Details), new { id = allocation.CustomerID });
//        }

//        // --------------------------
//        // Customer Details (Allocations)
//        // --------------------------
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null) return NotFound();

//            var customer = await _customerService.GetCustomerDetailsAsync(id.Value); // ✅ service
//            if (customer == null) return NotFound();

//            var fridgeAllocationsVM = customer.FridgeAllocation?
//                .Select(a => new FridgeAllocationViewModel
//                {
//                    AllocationID = a.AllocationID,
//                    FridgeId = a.FridgeId,
//                    AllocationDate = a.AllocationDate,
//                    ReturnDate = a.ReturnDate,
//                    Status = a.Status,
//                    QuantityAllocated = a.QuantityAllocated,
//                    Fridge = new FridgeViewModel
//                    {
//                        FridgeId = a.FridgeId,
//                        Brand = a.Fridge?.Brand,
//                        Model = a.Fridge?.Model,
//                        Status = a.Fridge?.Status
//                    }
//                }).ToList() ?? new List<FridgeAllocationViewModel>();

//            var customerVM = new CustomerViewModel
//            {
//                CustomerId = customer.CustomerID,
//                FullNames = customer.FullName,
//                LocationId = customer.LocationId,
//                PhoneNumber = customer.PhoneNumber,
//                IsActive = customer.IsActive,
//                RegistrationDate = customer.RegistrationDate,
//                FridgeAllocations = fridgeAllocationsVM
//            };

//            return View(customerVM);
//        }

//        public async Task<IActionResult> ProcessPendingAllocations()
//{
//    // Get all OrderItems with linked Orders and Fridges
//    var pendingItems = await _context.OrderItems
//        .Include(oi => oi.Order)
//            .ThenInclude(o => o.Customers)
//        .Include(oi => oi.Fridge)
//        .Where(oi => oi.Order != null && oi.Fridge != null)
//        .ToListAsync();

//    Console.WriteLine($"Found {pendingItems.Count} order items");

//    // Build the ViewModel with DEBUGGING
//    var viewModel = new List<PendingAllocationViewModel>();

//    foreach (var oi in pendingItems)
//    {
//        var customerId = oi.Order?.CustomerID ?? 0;
//        var customerName = oi.Order?.Customers?.FullName ?? "Unknown";

//        // DEBUG: Check allocation data
//        var allocations = await _context.FridgeAllocation
//            .Where(fa => fa.FridgeId == oi.FridgeId && fa.CustomerID == customerId)
//            .ToListAsync();

//        Console.WriteLine($"OrderItem {oi.OrderItemId}: Customer {customerId} ({customerName}), Fridge {oi.FridgeId}");
//        Console.WriteLine($"Found {allocations.Count} allocations for this customer+fridge combo");
//        foreach (var alloc in allocations)
//        {
//            Console.WriteLine($"  Allocation: {alloc.QuantityAllocated} units");
//        }

//        var totalAllocated = allocations.Sum(a => a.QuantityAllocated);
//        var quantityOrdered = oi.Quantity;
//        var quantityPending = quantityOrdered - totalAllocated;

//        var vm = new PendingAllocationViewModel
//        {
//            OrderItemId = oi.OrderItemId,
//            CustomerId = customerId,
//            CustomerName = customerName,
//            FridgeId = oi.FridgeId,
//            FridgeBrand = oi.Fridge?.Brand ?? "N/A",
//            FridgeModel = oi.Fridge?.Model ?? "N/A",
//            QuantityOrdered = quantityOrdered,
//            QuantityAllocated = totalAllocated,
//            QuantityPending = quantityPending,
//            Status = totalAllocated >= quantityOrdered ? "Allocated" : "Pending"
//        };

//        viewModel.Add(vm);

//        Console.WriteLine($"Result: Ordered={quantityOrdered}, Allocated={totalAllocated}, Pending={quantityPending}, Status={vm.Status}");
//        Console.WriteLine("---");
//    }

//    return View(viewModel);
//}


//        // --------------------------
//        // Search Customers
//        // --------------------------
//        public async Task<IActionResult> Search(string searchString)
//        {
//            var customers = await _customerService.GetAllCustomersWithFridgesAsync(); // ✅ service

//            if (!string.IsNullOrEmpty(searchString))
//            {
//                searchString = searchString.ToLower();
//                customers = customers
//                    .Where(c =>
//                        c.FullName.ToLower().Contains(searchString) ||
//                        c.Email.ToLower().Contains(searchString) ||
//                        c.PhoneNumber.ToLower().Contains(searchString))
//                    .ToList();
//            }

//            var model = customers.Select(c => new CustomerViewModel
//            {
//                CustomerId = c.CustomerID,
//                FullNames = c.FullName,
//                LocationId = c.LocationId,
//                PhoneNumber = c.PhoneNumber,
//                IsActive = c.IsActive,
//                RegistrationDate = c.RegistrationDate,
//                FridgeAllocations = c.FridgeAllocation.Select(a => new FridgeAllocationViewModel
//                {
//                    AllocationID = a.AllocationID,
//                    FridgeId = a.FridgeId,
//                    AllocationDate = a.AllocationDate,
//                    ReturnDate = a.ReturnDate,
//                    Status = a.Status,
//                    QuantityAllocated = a.QuantityAllocated,
//                    Fridge = new FridgeViewModel
//                    {
//                        FridgeId = a.FridgeId,
//                        Brand = a.Fridge?.Brand,
//                        Model = a.Fridge?.Model,
//                        Status = a.Fridge?.Status
//                    }
//                }).ToList()
//            }).ToList();

//            return View("Index", model);
//        }

//        public IActionResult ExportToPdf(int id)
//        {
//            var customer = _context.Customers
//                .Include(c => c.FridgeAllocation)
//                .ThenInclude(a => a.Fridge)
//                .FirstOrDefault(c => c.CustomerID == id);

//            if (customer == null) return NotFound();

//            QuestPDF.Settings.License = LicenseType.Community;

//            var pdfBytes = Document.Create(container =>
//            {
//                container.Page(page =>
//                {
//                    page.Size(PageSizes.A4);
//                    page.Margin(2, Unit.Centimetre);
//                    page.DefaultTextStyle(x => x.FontSize(12));

//                    page.Content().Column(col =>
//                    {
//                        col.Item().Text($"Customer: {customer.FullName}").Bold().FontSize(14);
//                        col.Item().Text($"Phone: {customer.PhoneNumber}");
//                        col.Item().Text($"Email: {customer.Email}");
//                        col.Item().Text($"Address: {customer.LocationId}");
//                        col.Item().Text($"Status: {(customer.IsActive ? "Active" : "Inactive")}");
//                        col.Item().Text($"Registration Date: {customer.RegistrationDate:yyyy/MM/dd}");
//                        col.Item().Text($"Total Fridges Allocated: {customer.FridgeAllocation.Sum(a => a.QuantityAllocated)}");

//                        col.Item().Text("\nFridge Allocations:").Bold().FontSize(13);

//                        col.Item().Table(table =>
//                        {
//                            table.ColumnsDefinition(columns =>
//                            {
//                                columns.RelativeColumn(3);
//                                columns.RelativeColumn(1);
//                                columns.RelativeColumn(2);
//                                columns.RelativeColumn(2);
//                                columns.RelativeColumn(1);
//                            });

//                            table.Header(header =>
//                            {
//                                header.Cell().Text("Brand & Model").Bold();
//                                header.Cell().Text("Quantity").Bold();
//                                header.Cell().Text("Allocation Date").Bold();
//                                header.Cell().Text("Return Date").Bold();
//                                header.Cell().Text("Status").Bold();
//                            });

//                            foreach (var allocation in customer.FridgeAllocation)
//                            {
//                                table.Cell().Text($"{allocation.Fridge?.Brand} - {allocation.Fridge?.Model}");
//                                table.Cell().Text(allocation.QuantityAllocated.ToString());
//                                table.Cell().Text(allocation.AllocationDate.ToString("yyyy/MM/dd"));
//                                table.Cell().Text(allocation.ReturnDate?.ToString("yyyy/MM/dd") ?? "N/A");
//                                table.Cell().Text(allocation.Status);
//                            }
//                        });
//                    });
//                });
//            }).GeneratePdf();

//            return File(pdfBytes, "application/pdf", $"Customer_{customer.CustomerID}_Allocations.pdf");
//        }
//    }
//}



//using FridgeManagementSystem.Data;
//using FridgeManagementSystem.Models;
//using FridgeManagementSystem.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using QuestPDF.Fluent;
//using QuestPDF.Helpers;
//using QuestPDF.Infrastructure;
//using Rotativa.AspNetCore;
//using System.IO;


//namespace CustomerManagementSubSystem.Controllers
//{
//    [Area("CustomerManagementSubSystem")]
//    public class CustomerLiaisonController : Controller
//    {
//        private readonly FridgeDbContext _context;
//        private readonly IMaintenanceRequestService _mrService;
//        public CustomerLiaisonController(FridgeDbContext context, IMaintenanceRequestService mrService)
//        {
//            _context = context;
//            _mrService = mrService;
//        }

//        // --------------------------
//        // List Customers (Active + Inactive)
//        // --------------------------
//        public async Task<IActionResult> Index()
//        {
//            var customers = await _context.Customers
//                                          .IgnoreQueryFilters()
//                                          .Include(c => c.FridgeAllocation)
//                                          .ThenInclude(a => a.Fridge)
//                                          .ToListAsync();

//            var model = customers.Select(c => new CustomerViewModel
//            {
//                CustomerId = c.CustomerID,
//                FullNames = c.FullName,
//                LocationId = c.LocationId,
//                PhoneNumber = c.PhoneNumber,
//                Email = c.Email,
//                IsActive = c.IsActive,
//                RegistrationDate = c.RegistrationDate,
//                FridgeAllocations = c.FridgeAllocation
//                                     .Select(a => new FridgeAllocationViewModel
//                                     {
//                                         AllocationID = a.AllocationID,
//                                         FridgeId = a.FridgeId,
//                                         QuantityAllocated = a.QuantityAllocated,
//                                         AllocationDate = a.AllocationDate,
//                                         ReturnDate = a.ReturnDate,
//                                         Status = a.Status,
//                                         Fridge = new FridgeViewModel
//                                         {
//                                             FridgeId = a.FridgeId,
//                                             Brand = a.Fridge?.Brand,
//                                             Model = a.Fridge?.Model,
//                                             Status = a.Fridge?.Status
//                                         }
//                                     }).ToList()
//            }).ToList();

//            return View(model);
//        }

//        // --------------------------
//        // Create Customer
//        // --------------------------
//        public IActionResult Create()
//        {
//            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "LocationName");
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Customer customer)
//        {
//            if (!ModelState.IsValid)
//            {
//                // reload the dropdown if model validation fails
//                ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "LocationName");
//                return View(customer);
//            }

//            customer.IsActive = true;
//            customer.RegistrationDate = DateOnly.FromDateTime(DateTime.Now);

//            _context.Customers.Add(customer);
//            await _context.SaveChangesAsync();

//            TempData["SuccessMessage"] = "Customer created successfully!";
//            return RedirectToAction(nameof(Index));
//        }

//        // --------------------------
//        // Edit Customer
//        // --------------------------
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null) return NotFound();

//            var customer = await _context.Customers.FindAsync(id);
//            if (customer == null || !customer.IsActive) return NotFound();

//            return View(customer);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(Customer customer)
//        {
//            if (!ModelState.IsValid)
//                return View(customer);

//            try
//            {
//                _context.Update(customer);
//                await _context.SaveChangesAsync();
//                TempData["SuccessMessage"] = "Customer updated successfully!";
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!_context.Customers.Any(e => e.CustomerID == customer.CustomerID))
//                    return NotFound();
//                else throw;
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        // --------------------------
//        // Delete Customer (Soft Delete)
//        // --------------------------
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null) return NotFound();

//            var customer = await _context.Customers
//                                         .FirstOrDefaultAsync(c => c.CustomerID == id && c.IsActive);

//            if (customer == null) return NotFound();

//            return View(customer);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int CustomerID)
//        {
//            var customer = await _context.Customers.FindAsync(CustomerID);
//            if (customer != null)
//            {
//                customer.IsActive = false;
//                await _context.SaveChangesAsync();
//                TempData["SuccessMessage"] = "Customer deleted successfully!";
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        // --------------------------
//        // Allocate Fridge to Customer
//        // --------------------------
//        public async Task<IActionResult> Allocate(int customerId)
//        {
//            var customer = await _context.Customers
//                                         .Include(c => c.FridgeAllocation)
//                                         .FirstOrDefaultAsync(c => c.CustomerID == customerId);

//            if (customer == null) return NotFound();

//            var model = new CustomerAllocationViewModel
//            {
//                CustomerId = customer.CustomerID,
//                CustomerName = customer.FullName,
//                Status = "Allocated",
//                AvailableFridges = await _context.Fridge
//                                                .Where(f => f.IsActive && f.Status == "Available" && f.Quantity > 0)
//                                                .ToListAsync()
//            };

//            return View(model);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Allocate(CustomerAllocationViewModel model)
//        {
//            // Validation checks
//            if (model.SelectedFridgeID == 0)
//                ModelState.AddModelError("", "Please select a fridge to allocate.");

//            if (model.QuantityAllocated <= 0)
//                ModelState.AddModelError("", "Quantity allocated must be greater than zero.");

//            if (model.ReturnDate < DateOnly.FromDateTime(DateTime.Now))
//                ModelState.AddModelError("", "Return date cannot be in the past.");

//            if (!ModelState.IsValid)
//            {
//                model.AvailableFridges = _context.Fridge
//                                                 .Where(f => f.IsActive && f.Status != "Scrapped" && f.Quantity > 0)
//                                                 .ToList();
//                return View(model);
//            }

//            var customer = _context.Customers.Find(model.CustomerId);
//            if (customer == null)
//            {
//                TempData["ErrorMessage"] = "Customer not found.";
//                return RedirectToAction("Index");
//            }

//            var fridge = _context.Fridge
//                .Find(model.SelectedFridgeID);

//            if (fridge == null || !fridge.IsActive || fridge.Status == "Scrapped")
//            {
//                TempData["ErrorMessage"] = "Selected fridge is invalid.";
//                return RedirectToAction("Index");
//            }

//            if (fridge.Quantity < model.QuantityAllocated)
//            {
//                ModelState.AddModelError("", $"Not enough stock available. Only {fridge.Quantity} left.");
//                model.AvailableFridges = _context.Fridge
//                                                 .Where(f => f.IsActive && f.Status != "Scrapped" && f.Quantity > 0)
//                                                 .ToList();
//                return View(model);
//            }

//            // Create allocation
//            var allocation = new FridgeAllocation
//            {
//                CustomerID = model.CustomerId,
//                FridgeId = model.SelectedFridgeID,
//                AllocationDate = DateOnly.FromDateTime(DateTime.Now),
//                ReturnDate = model.ReturnDate,
//                Status = "Allocated",
//                QuantityAllocated = model.QuantityAllocated
//            };

//            _context.FridgeAllocation.Add(allocation);

//            // Update fridge stock
//            fridge.Quantity -= model.QuantityAllocated;
//            fridge.Status = fridge.Quantity > 0 ? "Available" : "Allocated";
//            _context.Fridge.Update(fridge);

//            _context.SaveChanges();
//            // Create initial maintenance request 

//            try
//            {
//                var created = _mrService.CreateInitialRequestForAllocationAsync(fridge.FridgeId)
//                                        .GetAwaiter()
//                                        .GetResult(); // blocks until async completes
//                if (created != null)
//                    TempData["Message"] = "Initial maintenance request created for this allocation.";
//                else
//                    TempData["Message"] = "No new maintenance request created (one already exists).";
//            }
//            catch (Exception ex)
//            {
//                TempData["Message"] = $"Failed to create initial maintenance request: {ex.Message}";
//            }
//            TempData["SuccessMessage"] = $"Allocated {model.QuantityAllocated} fridge(s) successfully!";
//            return RedirectToAction("Details", new { id = model.CustomerId });



//        }

//        // --------------------------
//        // Return Fridge
//        // --------------------------
//        public async Task<IActionResult> ReturnFridge(int allocationId)
//        {
//            var allocation = await _context.FridgeAllocation
//                                           .Include(a => a.Fridge)
//                                           .FirstOrDefaultAsync(a => a.AllocationID == allocationId);

//            if (allocation == null || allocation.Status != "Allocated")
//                return NotFound();

//            allocation.Status = "Returned";
//            allocation.ReturnDate = DateOnly.FromDateTime(DateTime.Now);

//            if (allocation.Fridge != null)
//                allocation.Fridge.Status = "Available";

//            await _context.SaveChangesAsync();
//            TempData["SuccessMessage"] = "Fridge returned successfully!";
//            return RedirectToAction(nameof(Details), new { id = allocation.CustomerID });
//        }

//        // --------------------------
//        // Scrap Fridge
//        // --------------------------
//        public async Task<IActionResult> ScrapFridge(int allocationId)
//        {
//            var allocation = await _context.FridgeAllocation
//                                           .Include(a => a.Fridge)
//                                           .FirstOrDefaultAsync(a => a.AllocationID == allocationId);

//            if (allocation == null || allocation.Status != "Allocated")
//                return NotFound();

//            allocation.Status = "Scrapped";
//            allocation.ReturnDate = DateOnly.FromDateTime(DateTime.Now);

//            if (allocation.Fridge != null)
//                allocation.Fridge.Status = "Scrapped";

//            await _context.SaveChangesAsync();
//            TempData["SuccessMessage"] = "Fridge scrapped successfully!";
//            return RedirectToAction(nameof(Details), new { id = allocation.CustomerID });
//        }

//        // --------------------------
//        // Customer Details (Allocations)
//        // --------------------------
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null) return NotFound();

//            var customer = await _context.Customers
//                                         .IgnoreQueryFilters()
//                                         .Include(c => c.FridgeAllocation)
//                                         .ThenInclude(a => a.Fridge)
//                                         .FirstOrDefaultAsync(c => c.CustomerID == id);

//            if (customer == null) return NotFound();

//            var fridgeAllocationsVM = customer.FridgeAllocation?
//                .Select(a => new FridgeAllocationViewModel
//                {
//                    AllocationID = a.AllocationID,
//                    FridgeId = a.FridgeId,
//                    AllocationDate = a.AllocationDate,
//                    ReturnDate = a.ReturnDate,
//                    Status = a.Status,
//                    QuantityAllocated = a.QuantityAllocated,
//                    Fridge = new FridgeViewModel
//                    {
//                        FridgeId = a.FridgeId,
//                        Brand = a.Fridge?.Brand,
//                        Model = a.Fridge?.Model,
//                        Status = a.Fridge?.Status
//                    }
//                }).ToList() ?? new List<FridgeAllocationViewModel>();

//            var customerVM = new CustomerViewModel
//            {
//                CustomerId = customer.CustomerID,
//                FullNames = customer.FullName,
//                LocationId = customer.LocationId,
//                PhoneNumber = customer.PhoneNumber,
//                IsActive = customer.IsActive,
//                RegistrationDate = customer.RegistrationDate,
//                FridgeAllocations = fridgeAllocationsVM
//            };

//            return View(customerVM);
//        }

//        // --------------------------
//        // Search Customers
//        // --------------------------
//        public async Task<IActionResult> Search(string searchString)
//        {
//            var query = _context.Customers
//                                .IgnoreQueryFilters()
//                                .Include(c => c.FridgeAllocation)
//                                .ThenInclude(a => a.Fridge)
//                                .AsQueryable();

//            if (!string.IsNullOrEmpty(searchString))
//            {
//                searchString = searchString.ToLower();
//                query = query.Where(c =>
//                    c.FullName.ToLower().Contains(searchString) ||
//                    c.Email.ToLower().Contains(searchString) ||
//                    c.PhoneNumber.ToLower().Contains(searchString));
//            }

//            var result = await query.ToListAsync();

//            var model = result.Select(c => new CustomerViewModel
//            {
//                CustomerId = c.CustomerID,
//                FullNames = c.FullName,
//                LocationId = c.LocationId,
//                PhoneNumber = c.PhoneNumber,
//                IsActive = c.IsActive,
//                RegistrationDate = c.RegistrationDate,
//                FridgeAllocations = c.FridgeAllocation
//                                     .Select(a => new FridgeAllocationViewModel
//                                     {
//                                         AllocationID = a.AllocationID,
//                                         FridgeId = a.FridgeId,
//                                         AllocationDate = a.AllocationDate,
//                                         ReturnDate = a.ReturnDate,
//                                         Status = a.Status,
//                                         QuantityAllocated = a.QuantityAllocated,
//                                         Fridge = new FridgeViewModel
//                                         {
//                                             FridgeId = a.FridgeId,
//                                             Brand = a.Fridge?.Brand,
//                                             Model = a.Fridge?.Model,
//                                             Status = a.Fridge?.Status
//                                         }
//                                     }).ToList()
//            }).ToList();

//            return View("Index", model);
//        }

//        public IActionResult ExportToPdf(int id)
//        {
//            var customer = _context.Customers
//                .Include(c => c.FridgeAllocation)
//                .ThenInclude(a => a.Fridge)
//                .FirstOrDefault(c => c.CustomerID == id);

//            if (customer == null) return NotFound();

//            // Set license to Community (free for evaluation/testing)
//            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

//            // Generate PDF
//            var pdfBytes = Document.Create(container =>
//            {
//                container.Page(page =>
//                {
//                    page.Size(PageSizes.A4);
//                    page.Margin(2, Unit.Centimetre);
//                    page.DefaultTextStyle(x => x.FontSize(12));

//                    page.Content().Column(col =>
//                    {
//                        col.Item().Text($"Customer: {customer.FullName}").Bold().FontSize(14);
//                        col.Item().Text($"Phone: {customer.PhoneNumber}");
//                        col.Item().Text($"Email: {customer.Email}");
//                        col.Item().Text($"Address: {customer.LocationId}");
//                        col.Item().Text($"Status: {(customer.IsActive ? "Active" : "Inactive")}");
//                        col.Item().Text($"Registration Date: {customer.RegistrationDate:yyyy/MM/dd}");
//                        col.Item().Text($"Total Fridges Allocated: {customer.FridgeAllocation.Sum(a => a.QuantityAllocated)}");

//                        col.Item().Text("\nFridge Allocations:").Bold().FontSize(13);

//                        col.Item().Table(table =>
//                        {
//                            // Use relative widths to avoid layout exceptions
//                            table.ColumnsDefinition(columns =>
//                            {
//                                columns.RelativeColumn(3); // Brand & Model
//                                columns.RelativeColumn(1); // Quantity
//                                columns.RelativeColumn(2); // Allocation Date
//                                columns.RelativeColumn(2); // Return Date
//                                columns.RelativeColumn(1); // Status
//                            });

//                            // Table header
//                            table.Header(header =>
//                            {
//                                header.Cell().Text("Brand & Model").Bold();
//                                header.Cell().Text("Quantity").Bold();
//                                header.Cell().Text("Allocation Date").Bold();
//                                header.Cell().Text("Return Date").Bold();
//                                header.Cell().Text("Status").Bold();
//                            });

//                            // Table rows
//                            foreach (var allocation in customer.FridgeAllocation)
//                            {
//                                table.Cell().Text($"{allocation.Fridge?.Brand} - {allocation.Fridge?.Model}");
//                                table.Cell().Text(allocation.QuantityAllocated.ToString());
//                                table.Cell().Text(allocation.AllocationDate.ToString("yyyy/MM/dd"));
//                                table.Cell().Text(allocation.ReturnDate?.ToString("yyyy/MM/dd") ?? "N/A");
//                                table.Cell().Text(allocation.Status);
//                            }
//                        });
//                    });
//                });
//            }).GeneratePdf(); // Generate PDF as byte array

//            // Return file for download
//            return File(pdfBytes, "application/pdf", $"Customer_{customer.CustomerID}_Allocations.pdf");
//        }
//    }

//}

