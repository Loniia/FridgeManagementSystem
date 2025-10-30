using FridgeManagementSystem.Data;
using FridgeManagementSystem.Helpers;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using FridgeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Rotativa.AspNetCore;
using System.IO;
#nullable disable

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = Roles.Admin)]
    public class ManageCustomerController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly FridgeDbContext _context;
        private readonly IMaintenanceRequestService _mrService;
        private readonly CustomerService _customerService;   // ✅ use shared service
        private readonly UserManager<ApplicationUser> _userManager;
        public ManageCustomerController(
            FridgeDbContext context,
            IMaintenanceRequestService mrService,
            CustomerService customerService, INotificationService notificationService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mrService = mrService;
            _customerService = customerService;
            _notificationService = notificationService;
            _userManager = userManager;
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
                IsVerified = c.IsVerified,
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

        [HttpPost]
        public async Task<IActionResult> VerifyCustomer(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return NotFound();

            customer.IsVerified = true;
            await _context.SaveChangesAsync();

            // Notify customer
            await _notificationService.CreateAsync(
                customer.ApplicationUserId.Value,
                "Your registration has been approved. You can now log in."
            );

            TempData["Message"] = $"Customer {customer.FullName} approved.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RejectCustomer(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                return NotFound();

            // Update rejection info
            customer.IsVerified = false;
            customer.IsActive = false;
            customer.UpdatedAt = DateTime.Now; // optional if you added it



            await _context.SaveChangesAsync();
            // Notify customer
            await _notificationService.CreateAsync(
                customer.ApplicationUserId.Value,
                "Your registration has been rejected. Please contact support."
            );
            TempData["Message"] = $"Customer {customer.FullName} has been rejected, and notified.";
            return RedirectToAction(nameof(Index));
        }



        // --------------------------
        // Create Customer
        // --------------------------
        public IActionResult Create()
        {
            PopulateLocations();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                PopulateLocations();
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
        // Private helper to populate locations dropdown
        // --------------------------
        private void PopulateLocations()
        {
            ViewBag.Locations = new SelectList(
                _context.Locations
                        .Where(l => l.IsActive)
                        .Select(l => new
                        {
                            l.LocationId,
                            FullAddress = l.Address + ", " + l.City + ", " + l.Province
                        })
                        .ToList(),
                "LocationId",
                "FullAddress"
            );
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

        //// --------------------------
        //// Scrap Fridge
        //// --------------------------
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
                Email = customer.Email,
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
                                table.Cell().Text(
                                  allocation.AllocationDate.HasValue
                                  ? allocation.AllocationDate.Value.ToString("yyyy/MM/dd")
                                  : "-"
                                );
                                table.Cell().Text(allocation.ReturnDate?.ToString("yyyy/MM/dd") ?? "N/A");
                                table.Cell().Text(allocation.Status);
                            }
                        });
                    });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Customer_{customer.CustomerID}_Allocations.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> PendingPayments()
        {
            var pendingPayments = await _context.Payments
                .Include(p => p.Orders)
                .ThenInclude(o => o.Customers)
                .Where(p => p.Status == "Pending" || p.Status == "Awaiting Verification" || p.Status == "AwaitingAdminApproval")
                .ToListAsync();

            return View(pendingPayments);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPayment(int paymentId, bool approve)
        {
            // 1) Load payment (minimal).
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null)
                return NotFound();

            // 2) Load the order
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == payment.OrderId);

            if (order == null)
                return BadRequest("Associated order not found.");

            // 3) Get CustomerID (from Orders table)
            var customerId = order.CustomerID;

            // ✅ NEW: Find the Identity User ID for notifications
            var customerAppUserIdNullable = await _context.Customers
    .Where(c => c.CustomerID == customerId)
    .Select(c => c.ApplicationUserId)
    .FirstOrDefaultAsync();

            // ✅ Check for null
            if (!customerAppUserIdNullable.HasValue)
                return BadRequest("Customer Identity user not found.");

            int customerAppUserId = customerAppUserIdNullable.Value;



            if (approve)
            {
                payment.Status = "Paid";
                order.Status = "Paid";

                await _context.SaveChangesAsync();

                // Clear cart after successful payment
                await CartHelper.ClearCartAsync(_context, customerId);

                // ✅ Send notification to the *customer’s Identity account*
                if (customerAppUserId != 0)
                {
                    await _notificationService.CreateAsync(
                        customerAppUserId,
                        $"Your payment for Order #{order.OrderId} has been approved ✅"
                    );
                }

                // ✅ Notify Admins
                var admins = await _userManager.GetUsersInRoleAsync("Admin");
                foreach (var admin in admins)
                {
                    await _notificationService.CreateAsync(
                        admin.Id,
                        $"Payment approved for Order #{order.OrderId} ✅"
                    );
                }
            }
            else
            {
                payment.Status = "Rejected";
                order.Status = "AwaitingPayment";

                await _context.SaveChangesAsync();

                // ✅ Send rejection notification to Customer (Identity)
                if (customerAppUserId != 0)
                {
                    await _notificationService.CreateAsync(
                        customerAppUserId,
                        $"Your payment for Order #{order.OrderId} has been rejected ❌ Please retry."
                    );
                }

                // ✅ Notify Admins a payment was rejected (optional but useful)
                var admins = await _userManager.GetUsersInRoleAsync("Admin");
                foreach (var admin in admins)
                {
                    await _notificationService.CreateAsync(
                        admin.Id,
                        $"Payment rejected for Order #{order.OrderId} ❌"
                    );
                }
            }

            // Reload payments safely
            await _context.Entry(order).Collection(o => o.Payments).LoadAsync();

            return RedirectToAction("PendingPayments");
        }

    }
}