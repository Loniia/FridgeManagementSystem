using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.ViewModels;
using TaskStatus = FridgeManagementSystem.Models.TaskStatus;
using QuestPDF.Fluent;

namespace FridgeManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly FridgeDbContext _context;
        private readonly FridgeService _fridgeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CustomerController> _logger;  // ✅ Add this line

        // Constructor
        public CustomerController(
            FridgeDbContext context,
            FridgeService fridgeService,
            UserManager<ApplicationUser> userManager,
            ILogger<CustomerController> logger) // ✅ Add logger to constructor
        {
            _context = context;
            _fridgeService = fridgeService;
            _userManager = userManager;
            _logger = logger;  // ✅ Initialize it
        }

        // ==========================
        // 1. DASHBOARD
        // ==========================
        public async Task<IActionResult> Dashboard(string search)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            // Convert to int
            if (!int.TryParse(userIdClaim.Value, out int userId))
                return RedirectToAction("Login", "Account");

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            // If customer record is missing, send back to login
            if (customer == null)
                return RedirectToAction("Login", "Account");

            // Start query for fridges
            var fridgesQuery = _context.Fridge
                .Include(f => f.FridgeAllocation)
                .AsQueryable();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(search))
            {
                fridgesQuery = fridgesQuery.Where(f =>
                    f.Brand.Contains(search) || f.Model.Contains(search));
            }

            ViewData["Search"] = search;

            var fridges = await fridgesQuery.ToListAsync();

            var fridgeViewModels = fridges.Select(f => new FridgeViewModel
            {
                FridgeId = f.FridgeId,
                FridgeType = f.FridgeType,
                Brand = f.Brand,
                Model = f.Model,
                Price = f.Price,
                Quantity = f.Quantity,
                ImageUrl = string.IsNullOrEmpty(f.ImageUrl) ? "/images/fridges/default.jpg" : f.ImageUrl,
                AvailableStock = f.Quantity - f.FridgeAllocation
                    .Where(a => a.ReturnDate == null || a.ReturnDate > DateOnly.FromDateTime(DateTime.Today))
                    .Count(),
                Status = (!f.IsActive || f.Quantity <= 0) ? "Out of Stock" : "In Stock"
            }).ToList();

            var model = new CustomerViewModel
            {
                FullNames = User.Identity?.Name ?? "Guest",
                Fridges = fridgeViewModels
            };

            return View(model);
        }

        // ==========================
        // 2. ADD TO CART
        // ==========================
        [HttpPost]
        public async Task<IActionResult> AddToCart(int fridgeId, int quantity = 1)
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

            if (cart == null)
            {
                cart = new Cart { CustomerID = customerId, IsActive = true };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var fridge = await _context.Fridge.FindAsync(fridgeId);
            if (fridge == null) return NotFound();

            // Try to find existing item even if it was soft-deleted
            var existingItem = cart.CartItems.FirstOrDefault(i => i.FridgeId == fridgeId);
            if (existingItem != null)
            {
                if (existingItem.IsDeleted)
                {
                    // Reactivate previously deleted item
                    existingItem.IsDeleted = false;
                    existingItem.Quantity = quantity; // reset to requested quantity OR keep previous? we set to requested
                    existingItem.Price = fridge.Price;
                    _context.CartItems.Update(existingItem);
                }
                else
                {
                    existingItem.Quantity += quantity;
                    existingItem.Price = fridge.Price;
                    _context.CartItems.Update(existingItem);
                }
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    FridgeId = fridgeId,
                    Quantity = quantity,
                    Price = fridge.Price,
                    IsDeleted = false
                });
            }

            // Ensure cart is active when adding items
            cart.IsActive = true;
            _context.Carts.Update(cart);

            await _context.SaveChangesAsync();
            return RedirectToAction("ViewCart");
        }

        // Remove from cart 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

            if (cart == null)
            {
                TempData["Error"] = "Cart not found.";
                return RedirectToAction("ViewCart");
            }

            var item = cart.CartItems.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (item == null)
            {
                TempData["Error"] = "Item not found in cart.";
                return RedirectToAction("ViewCart");
            }

            // Soft delete instead of removing
            item.IsDeleted = true;
            _context.CartItems.Update(item);

            await _context.SaveChangesAsync();
            TempData["Success"] = "Item removed from cart.";

            return RedirectToAction("ViewCart");
        }


        // ==========================
        // 3. VIEW CART
        // ==========================
        public async Task<IActionResult> ViewCart()
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems.Where(ci => !ci.IsDeleted))
                .ThenInclude(ci => ci.Fridge)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

            // If cart is null, return empty view model or redirect
            if (cart == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View(new Cart { CartItems = new List<CartItem>() });
            }

            return View(cart);
        }

        // ==========================
        // 4. CHECKOUT
        // ==========================
        public async Task<IActionResult> Checkout()
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            // Only include non-deleted items and ensure cart is active
            var cart = await _context.Carts
                .Include(c => c.CartItems.Where(ci => !ci.IsDeleted))
                .ThenInclude(i => i.Fridge)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId && c.IsActive);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("ViewCart");
            }

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckout(string deliveryAddress, string contactName, string phoneNumber)
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems.Where(ci => !ci.IsDeleted))
                .ThenInclude(i => i.Fridge)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId && c.IsActive);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("ViewCart");
            }

            var order = new Order
            {
                CustomerID = customerId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                DeliveryAddress = deliveryAddress,
                ContactName = contactName,
                ContactPhone = phoneNumber,
                TotalAmount = cart.CartItems.Sum(i => i.Price * i.Quantity)
            };

            foreach (var ci in cart.CartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    FridgeId = ci.FridgeId,
                    Quantity = ci.Quantity,
                    Price = ci.Price
                });
            }

            _context.Orders.Add(order);

            // Mark the cart as inactive (so new items require a new cart)
            cart.IsActive = false;
            _context.Carts.Update(cart);

            await _context.SaveChangesAsync();

            // Explicitly redirect to AddCard action in Customer controller
            return RedirectToAction("AddCard", "Customer", new { orderId = order.OrderId, amount = order.TotalAmount });
        }

        // ==========================
        // 5. ORDER CONFIRMATION
        // ==========================
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Fridge)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            return View(order);
        }

        // ==========================
        // 6. PAYMENT METHODS
        // ==========================
        [HttpGet]
        public async Task<IActionResult> AddCard(int orderId, decimal? amount)
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Fridge)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerID == customerId);

            if (order == null) return NotFound();

            var vm = new PaymentViewModel
            {
                OrderId = order.OrderId,
                Amount = amount ?? order.TotalAmount,
                Method = Method.Card,
                Orders = order
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCard(PaymentViewModel model)
        {
            // Debug: Log incoming model
            _logger.LogInformation($"AddCard POST - OrderId: {model.OrderId}, Amount: {model.Amount}, Method: {model.Method}");

            // Clear previous model state to start fresh
            ModelState.Clear();

            // Validate based on payment method
            if (model.Method == Method.Card)
            {
                if (string.IsNullOrWhiteSpace(model.CardholderName))
                    ModelState.AddModelError("CardholderName", "Cardholder Name is required.");

                if (string.IsNullOrWhiteSpace(model.CardNumber))
                    ModelState.AddModelError("CardNumber", "Card Number is required.");

                if (string.IsNullOrWhiteSpace(model.ExpiryDate))
                    ModelState.AddModelError("ExpiryDate", "Expiry Date is required.");

                if (string.IsNullOrWhiteSpace(model.CVV))
                    ModelState.AddModelError("CVV", "CVV is required.");
            }
            else if (model.Method == Method.EFT)
            {
                if (string.IsNullOrWhiteSpace(model.BankReference))
                    ModelState.AddModelError("BankReference", "Bank Reference is required.");
            }

            // If ModelState is invalid, return to form with errors
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"AddCard validation failed - Errors: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");

                // Reload the order to populate the view model
                var customerId = GetLoggedInCustomerId();
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Fridge)
                    .FirstOrDefaultAsync(o => o.OrderId == model.OrderId && o.CustomerID == customerId);

                model.Orders = order;
                return View(model);
            }

            try
            {
                var customerId = GetLoggedInCustomerId();
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.OrderId == model.OrderId && o.CustomerID == customerId);

                if (order == null || order.CustomerID != customerId)
                    return Forbid();

                // Create payment record
                var payment = new Payment
                {
                    OrderId = model.OrderId,
                    Amount = model.Amount,
                    Method = model.Method,
                    CardNumber = model.Method == Method.Card ? MaskCardNumber(model.CardNumber) : null,
                    BankReference = model.Method == Method.EFT ? model.BankReference : null,
                    PaymentDate = DateTime.Now,
                    Status = "Paid"
                };

                _context.Payments.Add(payment);

                // Update order status
                order.Status = "Paid";
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Payment successful for order {order.OrderId}");
                return RedirectToAction("PaymentConfirmation", "Customer", new { orderId = order.OrderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for order {OrderId}", model.OrderId);
                ModelState.AddModelError("", "An error occurred while processing your payment. Please try again.");

                // Reload the order
                var customerId = GetLoggedInCustomerId();
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Fridge)
                    .FirstOrDefaultAsync(o => o.OrderId == model.OrderId && o.CustomerID == customerId);

                model.Orders = order;
                return View(model);
            }
        }

        // ==========================
        // 7. PAYMENT CONFIRMATION
        // ==========================
        [HttpGet]
        public async Task<IActionResult> PaymentConfirmation(int orderId)
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Fridge)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerID == customerId);

            if (order == null)
            {
                return NotFound();
            }

            var payment = order.Payments?.FirstOrDefault();

            var viewModel = new PaymentConfirmationViewModel
            {
                OrderId = order.OrderId,
                Amount = payment?.Amount ?? order.TotalAmount,
                PaymentDate = payment?.PaymentDate ?? DateTime.Now,
                PaymentMethod = payment?.Method.ToString() ?? "Card",
                Status = payment?.Status ?? "Paid",
                OrderItems = order.OrderItems?.ToList() ?? new List<OrderItem>()
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> OrderHistory()
        {
            var appUser = await _userManager.GetUserAsync(User);
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.ApplicationUserId == appUser.Id);

            if (customer == null)
                return NotFound("Customer profile not found.");

            var orders = await _context.Orders
                .Where(o => o.CustomerID == customer.CustomerID) 
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Fridge)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        [Authorize]
        public async Task<IActionResult> ViewOrder(int id)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.ApplicationUserId == appUser.Id);

            if (customer == null)
                return NotFound("Customer profile not found.");

            var order = await _context.Orders
                .Where(o => o.OrderId == id && o.CustomerID == customer.CustomerID)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Fridge)
                .FirstOrDefaultAsync();

            return View(order);
        }

        // ==========================
        // 7. MY ACCOUNT & ORDERS
        // ==========================
        public async Task<IActionResult> MyAccount()
        {
            var customerId = GetLoggedInCustomerId();
            var orders = await _context.Orders
                .Where(o => o.CustomerID == customerId)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Fridge)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> TrackOrder(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            return View(order);
        }

        // ==========================
        // 8. FAULT MANAGEMENT
        // ==========================
        public async Task<IActionResult> MyFaults()
        {
            try
            {
                var customerId = GetLoggedInCustomerId();
                var faults = await _context.Faults
                    .Include(f => f.Fridge)
                    .Include(f => f.AssignedTechnician)
                    .Where(f => f.CustomerId == customerId)
                    .OrderByDescending(f => f.FaultID)
                    .ToListAsync();

                return View(faults);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error loading faults.";
                return View(new List<Fault>());
            }
        }

        public async Task<IActionResult> FaultDetails(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var customerId = GetLoggedInCustomerId();
                var fault = await _context.Faults
                    .Include(f => f.Fridge)
                    .Include(f => f.AssignedTechnician)
                    .Include(f => f.RepairSchedules)
                    .FirstOrDefaultAsync(f => f.FaultID == id && f.CustomerId == customerId);

                return fault == null ? NotFound() : View(fault);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error loading fault details.";
                return RedirectToAction(nameof(MyFaults));
            }
        }

        public IActionResult CreateFault() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFault(CreateFaultViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            try
            {
                var fault = new Fault
                {
                    FridgeId = viewModel.FridgeId,
                    Priority = viewModel.Priority,
                    FaultDescription = viewModel.FaultDescription,
                    CustomerId = GetLoggedInCustomerId(),
                    Status = "Pending",
                    ReportDate = DateTime.Now,
                    FaultCode = GenerateFaultCode()
                };

                _context.Add(fault);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fault reported successfully.";
                return RedirectToAction(nameof(FaultDetails), new { id = fault.FaultID });
            }
            catch
            {
                TempData["ErrorMessage"] = "Error reporting fault.";
                return View(viewModel);
            }
        }


        // ==========================
        // 10. SERVICE HISTORY
        // ==========================
        public async Task<IActionResult> UpcomingVisits()
        {
            var customerId = GetLoggedInCustomerId();
            var visits = await _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                .ThenInclude(r => r.Fridge)
                .Include(v => v.Employee)
                .Where(v => v.MaintenanceRequest.Fridge.CustomerID == customerId &&
                            (v.Status == TaskStatus.Scheduled || v.Status == TaskStatus.Rescheduled))
                .OrderBy(v => v.ScheduledDate)
                .ThenBy(v => v.ScheduledTime)
                .ToListAsync();

            return View(visits);
        }

        public async Task<IActionResult> FridgeServiceHistory(int fridgeId)
        {
            var customerId = GetLoggedInCustomerId();
            var visits = await _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                .ThenInclude(r => r.Fridge)
                .Include(v => v.Employee)
                .Include(v => v.MaintenanceChecklist)
                .Include(v => v.ComponentUsed)
                .Include(v => v.FaultReport)
                .Where(v => v.MaintenanceRequest.FridgeId == fridgeId &&
                            v.MaintenanceRequest.Fridge.CustomerID == customerId)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();

            return View(visits);
        }

        [HttpGet]
        public IActionResult DownloadFridgeServiceHistory(int fridgeId)
        {
            var customerId = GetLoggedInCustomerId();
            var fridge = _context.Fridge
                .Include(f => f.Customer)
                .FirstOrDefault(f => f.FridgeId == fridgeId && f.CustomerID == customerId);

            if (fridge == null) return NotFound("Fridge not found.");

            var visits = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                .Include(v => v.Employee)
                .Include(v => v.MaintenanceChecklist)
                .Include(v => v.ComponentUsed)
                .Include(v => v.FaultReport)
                .Where(v => v.MaintenanceRequest.FridgeId == fridgeId)
                .OrderByDescending(v => v.ScheduledDate)
                .ToList();

            if (!visits.Any()) return NotFound("No service history.");

            var generator = new ServiceHistoryPdfGenerator(visits, fridge);
            var pdfBytes = generator.GeneratePdf();
            var fileName = $"ServiceHistory_{fridge.Brand}_{DateTime.Now:yyyyMMdd}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }

        // ==========================
        // HELPER METHODS
        // ==========================
        private int GetLoggedInCustomerId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return 0;

            if (!int.TryParse(userIdClaim.Value, out var appUserId)) return 0;

            var customer = _context.Customers.FirstOrDefault(c => c.ApplicationUserId == appUserId);

            return customer?.CustomerID ?? 0;
        }

        private string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber)) return null;

            var cleaned = cardNumber.Replace(" ", "");
            var last4 = cleaned.Length >= 4 ? cleaned[^4..] : cleaned;

            return new string('*', Math.Max(0, cleaned.Length - 4)) + last4;
        }

        private string GenerateFaultCode()
        {
            return "FLT-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }

        private string GenerateRequestCode()
        {
            return "REQ-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }
    }
}








//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Identity;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System;
//using FridgeManagementSystem.Data;
//using FridgeManagementSystem.Models;
//using FridgeManagementSystem.ViewModels;
//using TaskStatus = FridgeManagementSystem.Models.TaskStatus;
//using QuestPDF.Fluent;

//namespace FridgeManagementSystem.Controllers
//{
//    public class CustomerController : Controller
//    {
//        private readonly FridgeDbContext _context;       
//        private readonly FridgeService _fridgeService;   
//        private readonly UserManager<ApplicationUser> _userManager; 

//        // Constructor
//        public CustomerController(
//            FridgeDbContext context,
//            FridgeService fridgeService,
//            UserManager<ApplicationUser> userManager)
//        {
//            _context = context;
//            _fridgeService = fridgeService;
//            _userManager = userManager;  
//        }

//        // ==========================
//        // 1. DASHBOARD
//        // ==========================
//        public async Task<IActionResult> Dashboard(string search)
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
//            if (userIdClaim == null)
//                return RedirectToAction("Login", "Account");

//            // Convert to int
//            if (!int.TryParse(userIdClaim.Value, out int userId))
//                return RedirectToAction("Login", "Account");

//            var customer = await _context.Customers
//                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);
//            if (customer != null && !customer.IsVerified && customer.IsActive)
//            {
//                // Redirect rejected customers to rejected dashboard
//                return RedirectToAction("RejectedCustomer", "Customer");
//            }

//            // Start query for fridges
//            var fridgesQuery = _context.Fridge
//                .Include(f => f.FridgeAllocation)
//                .AsQueryable();

//            // Apply search filter if provided
//            if (!string.IsNullOrEmpty(search))
//            {
//                fridgesQuery = fridgesQuery.Where(f =>
//                    f.Brand.Contains(search) || f.Model.Contains(search));
//            }

//            ViewData["Search"] = search;

//            var fridges = await fridgesQuery.ToListAsync();

//            var fridgeViewModels = fridges.Select(f => new FridgeViewModel
//            {
//                FridgeId = f.FridgeId,
//                FridgeType = f.FridgeType,
//                Brand = f.Brand,
//                Model = f.Model,
//                Price = f.Price,
//                Quantity = f.Quantity,
//                ImageUrl = string.IsNullOrEmpty(f.ImageUrl) ? "/images/fridges/default.jpg" : f.ImageUrl,
//                AvailableStock = f.Quantity - f.FridgeAllocation
//                    .Where(a => a.ReturnDate == null || a.ReturnDate > DateOnly.FromDateTime(DateTime.Today))
//                    .Count(),
//                Status = (!f.IsActive || f.Quantity <= 0) ? "Out of Stock" : "In Stock"
//            }).ToList();

//            var model = new CustomerViewModel
//            {
//                FullNames = User.Identity?.Name ?? "Guest",
//                Fridges = fridgeViewModels
//            };

//            return View(model);
//        }

//        // ==========================
//        // 2. ADD TO CART
//        // ==========================
//        [HttpPost]
//        public async Task<IActionResult> AddToCart(int fridgeId, int quantity = 1)
//        {
//            var customerId = GetLoggedInCustomerId();
//            if (customerId == 0) return RedirectToAction("Login", "Account");

//            var cart = await _context.Carts.Include(c => c.CartItems)
//                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

//            if (cart == null)
//            {
//                cart = new Cart { CustomerID = customerId };
//                _context.Carts.Add(cart);
//                await _context.SaveChangesAsync();
//            }

//            var fridge = await _context.Fridge.FindAsync(fridgeId);
//            if (fridge == null) return NotFound();

//            var existingItem = cart.CartItems.FirstOrDefault(i => i.FridgeId == fridgeId);
//            if (existingItem != null)
//            {
//                existingItem.Quantity += quantity;
//                existingItem.Price = fridge.Price;
//            }
//            else
//            {
//                cart.CartItems.Add(new CartItem { FridgeId = fridgeId, Quantity = quantity, Price = fridge.Price });
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction("ViewCart");
//        }
//        // Remove from cart 
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> RemoveFromCart(int cartItemId)
//        {
//            var customerId = GetLoggedInCustomerId();
//            if (customerId == 0) return RedirectToAction("Login", "Account");

//            var cart = await _context.Carts
//                .Include(c => c.CartItems)
//                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

//            if (cart == null)
//            {
//                TempData["Error"] = "Cart not found.";
//                return RedirectToAction("ViewCart");
//            }

//            var item = cart.CartItems.FirstOrDefault(i => i.CartItemId == cartItemId);
//            if (item == null)
//            {
//                TempData["Error"] = "Item not found in cart.";
//                return RedirectToAction("ViewCart");
//            }

//            // Soft delete instead of removing
//            item.IsDeleted = true;
//            _context.CartItems.Update(item);

//            await _context.SaveChangesAsync();
//            TempData["Success"] = "Item removed from cart.";

//            return RedirectToAction("ViewCart");
//        }


//        // ==========================
//        // 3. VIEW CART
//        // ==========================
//        public async Task<IActionResult> ViewCart()
//        {
//            var customerId = GetLoggedInCustomerId();

//            var cart = await _context.Carts
//                .Include(c => c.CartItems.Where(ci => !ci.IsDeleted)) // only active items
//                .ThenInclude(ci => ci.Fridge)
//                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

//            return View(cart);
//        }

//        // ==========================
//        // 4. CHECKOUT
//        // ==========================
//        public async Task<IActionResult> Checkout()
//        {
//            var customerId = GetLoggedInCustomerId();
//            var cart = await _context.Carts
//                .Include(c => c.CartItems)
//                .ThenInclude(i => i.Fridge)
//                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

//            if (cart == null || !cart.CartItems.Any())
//            {
//                TempData["Error"] = "Your cart is empty.";
//                return RedirectToAction("ViewCart");
//            }

//            return View(cart);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> ConfirmCheckout(string deliveryAddress, string contactName,string phoneNumber)
//        {
            
//            var customerId = GetLoggedInCustomerId();
//            if (customerId == 0) return RedirectToAction("Login", "Account");

//            var cart = await _context.Carts
//                .Include(c => c.CartItems.Where(ci => !ci.IsDeleted))
//                .ThenInclude(i => i.Fridge)
//                .FirstOrDefaultAsync(c => c.CustomerID == customerId && c.IsActive);

//            if (cart == null || !cart.CartItems.Any())
//            {
//                TempData["Error"] = "Your cart is empty.";
//                return RedirectToAction("ViewCart");
//            }

//            var order = new Order
//            {
//                CustomerID = customerId,
//                OrderDate = DateTime.Now,
//                Status = "Pending",
//                DeliveryAddress = deliveryAddress,
//                ContactName = contactName,
//                ContactPhone = phoneNumber,
//                TotalAmount = cart.CartItems.Sum(i => i.Price * i.Quantity)
//            };

//            foreach (var ci in cart.CartItems)
//            {
//                order.OrderItems.Add(new OrderItem
//                {
//                    FridgeId = ci.FridgeId,
//                    Quantity = ci.Quantity,
//                    Price = ci.Price
//                });
//            }

//            _context.Orders.Add(order);

//            // Soft delete instead of removing
//            cart.IsActive = false;
//            _context.Carts.Update(cart);

//            await _context.SaveChangesAsync();

//            return RedirectToAction("AddCard", new { orderId = order.OrderId, amount = order.TotalAmount });
//        }

//        // ==========================
//        // 5. ORDER CONFIRMATION
//        // ==========================
//        public async Task<IActionResult> OrderConfirmation(int id)
//        {
//            var order = await _context.Orders
//                .Include(o => o.OrderItems)
//                .ThenInclude(i => i.Fridge)
//                .FirstOrDefaultAsync(o => o.OrderId == id);

//            return View(order);
//        }

//        // ==========================
//        // 6. PAYMENT METHODS
//        // ==========================
//        [HttpGet]
//        public async Task<IActionResult> AddCard(int orderId, decimal? amount)
//        {
//            var customerId = GetLoggedInCustomerId();
//            if (customerId == 0) return RedirectToAction("Login", "Account");

//            var order = await _context.Orders
//                .Include(o => o.OrderItems)
//                .ThenInclude(i => i.Fridge)
//                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerID == customerId);

//            if (order == null) return NotFound();

//            var vm = new PaymentViewModel
//            {
//                OrderId = order.OrderId,
//                Amount = amount ?? order.TotalAmount,
//                Method = Method.Card,
//                Orders = order
//            };

//            return View(vm);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddCard(PaymentViewModel model)
//        {
//            // Validate card fields only if payment method is Card
//            if (model.Method == Method.Card)
//            {
//                if (string.IsNullOrWhiteSpace(model.CardholderName))
//                    ModelState.AddModelError("CardholderName", "Cardholder Name is required.");

//                if (string.IsNullOrWhiteSpace(model.CardNumber))
//                    ModelState.AddModelError("CardNumber", "Card Number is required.");

//                if (string.IsNullOrWhiteSpace(model.ExpiryDate))
//                    ModelState.AddModelError("ExpiryDate", "Expiry Date is required.");

//                if (string.IsNullOrWhiteSpace(model.CVV))
//                    ModelState.AddModelError("CVV", "CVV is required.");
//            }

//            // If ModelState is invalid, reload the form with validation messages
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            var customerId = GetLoggedInCustomerId();
//            var order = await _context.Orders
//                .Include(o => o.OrderItems)
//                .FirstOrDefaultAsync(o => o.OrderId == model.OrderId && o.CustomerID == customerId);

//            if (order == null || order.CustomerID != customerId)
//                return Forbid();

//            // Calculate amount from order items to ensure it's correct
//            model.Amount = await _context.OrderItems
//                .Where(oi => oi.OrderId == model.OrderId)
//                .SumAsync(oi => oi.Price * oi.Quantity);

//            var payment = new Payment
//            {
//                OrderId = model.OrderId,
//                Amount = model.Amount,
//                Method = model.Method,
//                CardNumber = model.Method == Method.Card ? MaskCardNumber(model.CardNumber) : null,
//                BankReference = model.Method != Method.Card ? model.BankReference : null,
//                PaymentDate = DateTime.Now,
//                Status = "Paid"
//            };

//            _context.Payments.Add(payment);

//            // Update order status
//            order.Status = "Paid";
//            await _context.SaveChangesAsync();

//            // Redirect to confirmation page
//            return RedirectToAction("PaymentConfirmation", "Customer", new { orderId = order.OrderId });
//        }

//        public IActionResult PaymentConfirmation() => View();

//        // ==========================
//        // 7. MY ACCOUNT & ORDERS
//        // ==========================
//        public async Task<IActionResult> MyAccount()
//        {
//            var customerId = GetLoggedInCustomerId();
//            var orders = await _context.Orders
//                .Where(o => o.CustomerID == customerId)
//                .Include(o => o.OrderItems)
//                .ThenInclude(i => i.Fridge)
//                .ToListAsync();

//            return View(orders);
//        }

//        public async Task<IActionResult> TrackOrder(int orderId)
//        {
//            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
//            return View(order);
//        }

//        // ==========================
//        // 8. FAULT MANAGEMENT
//        // ==========================
//        public async Task<IActionResult> MyFaults()
//        {
//            try
//            {
//                var customerId = GetLoggedInCustomerId();
//                var faults = await _context.Faults
//                    .Include(f => f.Fridge)
//                    .Include(f => f.AssignedTechnician)
//                    .Where(f => f.CustomerId == customerId)
//                    .OrderByDescending(f => f.FaultID)
//                    .ToListAsync();

//                return View(faults);
//            }
//            catch
//            {
//                TempData["ErrorMessage"] = "Error loading faults.";
//                return View(new List<Fault>());
//            }
//        }

//        public async Task<IActionResult> FaultDetails(int? id)
//        {
//            if (id == null) return NotFound();

//            try
//            {
//                var customerId = GetLoggedInCustomerId();
//                var fault = await _context.Faults
//                    .Include(f => f.Fridge)
//                    .Include(f => f.AssignedTechnician)
//                    .Include(f => f.RepairSchedules)
//                    .FirstOrDefaultAsync(f => f.FaultID == id && f.CustomerId == customerId);

//                return fault == null ? NotFound() : View(fault);
//            }
//            catch
//            {
//                TempData["ErrorMessage"] = "Error loading fault details.";
//                return RedirectToAction(nameof(MyFaults));
//            }
//        }

//        public IActionResult CreateFault() => View();

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> CreateFault(CreateFaultViewModel viewModel)
//        {
//            if (!ModelState.IsValid) return View(viewModel);

//            try
//            {
//                var fault = new Fault
//                {
//                    FridgeId = viewModel.FridgeId,
//                    Priority = viewModel.Priority,
//                    FaultDescription = viewModel.FaultDescription,
//                    CustomerId = GetLoggedInCustomerId(),
//                    Status = "Pending",
//                    ReportDate = DateTime.Now,
//                    FaultCode = GenerateFaultCode()
//                };

//                _context.Add(fault);
//                await _context.SaveChangesAsync();

//                TempData["SuccessMessage"] = "Fault reported successfully.";
//                return RedirectToAction(nameof(FaultDetails), new { id = fault.FaultID });
//            }
//            catch
//            {
//                TempData["ErrorMessage"] = "Error reporting fault.";
//                return View(viewModel);
//            }
//        }


//        // ==========================
//        // 10. SERVICE HISTORY
//        // ==========================
//        public async Task<IActionResult> UpcomingVisits()
//        {
//            var customerId = GetLoggedInCustomerId();
//            var visits = await _context.MaintenanceVisit
//                .Include(v => v.MaintenanceRequest)
//                .ThenInclude(r => r.Fridge)
//                .Include(v => v.Employee)
//                .Where(v => v.MaintenanceRequest.Fridge.CustomerID == customerId &&
//                            (v.Status == TaskStatus.Scheduled || v.Status == TaskStatus.Rescheduled))
//                .OrderBy(v => v.ScheduledDate)
//                .ThenBy(v => v.ScheduledTime)
//                .ToListAsync();

//            return View(visits);
//        }

//        public async Task<IActionResult> FridgeServiceHistory(int fridgeId)
//        {
//            var customerId = GetLoggedInCustomerId();
//            var visits = await _context.MaintenanceVisit
//                .Include(v => v.MaintenanceRequest)
//                .ThenInclude(r => r.Fridge)
//                .Include(v => v.Employee)
//                .Include(v => v.MaintenanceChecklist)
//                .Include(v => v.ComponentUsed)
//                .Include(v => v.FaultReport)
//                .Where(v => v.MaintenanceRequest.FridgeId == fridgeId &&
//                            v.MaintenanceRequest.Fridge.CustomerID == customerId)
//                .OrderByDescending(v => v.ScheduledDate)
//                .ToListAsync();

//            return View(visits);
//        }

//        [HttpGet]
//        public IActionResult DownloadFridgeServiceHistory(int fridgeId)
//        {
//            var customerId = GetLoggedInCustomerId();
//            var fridge = _context.Fridge
//                .Include(f => f.Customer)
//                .FirstOrDefault(f => f.FridgeId == fridgeId && f.CustomerID == customerId);

//            if (fridge == null) return NotFound("Fridge not found.");

//            var visits = _context.MaintenanceVisit
//                .Include(v => v.MaintenanceRequest)
//                .Include(v => v.Employee)
//                .Include(v => v.MaintenanceChecklist)
//                .Include(v => v.ComponentUsed)
//                .Include(v => v.FaultReport)
//                .Where(v => v.MaintenanceRequest.FridgeId == fridgeId)
//                .OrderByDescending(v => v.ScheduledDate)
//                .ToList();

//            if (!visits.Any()) return NotFound("No service history.");

//            var generator = new ServiceHistoryPdfGenerator(visits, fridge);
//            var pdfBytes = generator.GeneratePdf();
//            var fileName = $"ServiceHistory_{fridge.Brand}_{DateTime.Now:yyyyMMdd}.pdf";

//            return File(pdfBytes, "application/pdf", fileName);
//        }

//        // ==========================
//        // HELPER METHODS
//        // ==========================
//        private int GetLoggedInCustomerId()
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
//            if (userIdClaim == null) return 0;

//            var appUserId = int.Parse(userIdClaim.Value);
//            var customer = _context.Customers.FirstOrDefault(c => c.ApplicationUserId == appUserId);

//            return customer?.CustomerID ?? 0;
//        }

//        private string MaskCardNumber(string cardNumber)
//        {
//            if (string.IsNullOrEmpty(cardNumber)) return null;

//            var cleaned = cardNumber.Replace(" ", "");
//            var last4 = cleaned.Length >= 4 ? cleaned[^4..] : cleaned;

//            return new string('*', Math.Max(0, cleaned.Length - 4)) + last4;
//        }

//        private string GenerateFaultCode()
//        {
//            return "FLT-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
//        }

//        private string GenerateRequestCode()
//        {
//            return "REQ-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
//        }
//    }
//}