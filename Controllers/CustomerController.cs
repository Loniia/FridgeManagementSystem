using FridgeManagementSystem.Data;
using FridgeManagementSystem.Helpers;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using FridgeManagementSystem.Utilities;
using FridgeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskStatus = FridgeManagementSystem.Models.TaskStatus;

namespace FridgeManagementSystem.Controllers
{
    [Authorize(Roles = Roles.Customer)]
    public class CustomerController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly FridgeDbContext _context;
        private readonly FridgeService _fridgeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CustomerController> _logger;

        // Constructor
        public CustomerController(
            FridgeDbContext context,
            FridgeService fridgeService,
            UserManager<ApplicationUser> userManager,
            ILogger<CustomerController> logger, INotificationService notificationService)
        {
            _context = context;
            _fridgeService = fridgeService;
            _userManager = userManager;
            _logger = logger;
            _notificationService = notificationService;
        }

        //=================
        //DASHBOARD - FIXED
        //=================
        public async Task<IActionResult> Dashboard(string search, string stockFilter = "all")
        {
            var customerId = GetLoggedInCustomerId();
            // use identity user id for notifications
            var appUserId = await GetCurrentApplicationUserIdAsync();
            var unreadNotifications = await _notificationService.GetUnreadAsync(appUserId);
            ViewBag.UnreadCount = unreadNotifications.Count();

            if (customerId == 0)
                return RedirectToAction("Login", "Account");

            // Fetch all fridges from DB
            var fridgesQuery = _context.Fridge.AsQueryable();

            // Optional search
            if (!string.IsNullOrEmpty(search))
            {
                fridgesQuery = fridgesQuery.Where(f =>
                    (f.Brand != null && f.Brand.Contains(search)) ||
                    (f.Model != null && f.Model.Contains(search)));
            }

            // Apply stock filter
            if (!string.IsNullOrEmpty(stockFilter) && stockFilter != "all")
            {
                if (stockFilter == "in-stock")
                {
                    fridgesQuery = fridgesQuery.Where(f =>
                        f.Status.Equals("Available", StringComparison.OrdinalIgnoreCase) && f.Quantity > 0);
                }
                else if (stockFilter == "out-of-stock")
                {
                    fridgesQuery = fridgesQuery.Where(f =>
                        f.Status.Equals("Available", StringComparison.OrdinalIgnoreCase) && f.Quantity == 0);
                }
            }

            var fridges = await fridgesQuery.ToListAsync();

            foreach (var f in fridges)
            {
                _logger.LogInformation($"Fridge {f.Brand} {f.Model} - Status in DB: '{f.Status}', Quantity: {f.Quantity}");
            }

            // Map to ViewModel with CORRECTED stock status logic
            var fridgeViewModels = fridges.Select(f => new FridgeViewModel
            {
                FridgeId = f.FridgeId,
                Brand = f.Brand,
                Model = f.Model,
                FridgeType = f.FridgeType,
                Price = f.Price,
                Quantity = f.Quantity,
                // ✅ FIXED LOGIC: Only show "In Stock" for Available fridges with quantity > 0
                // For "Received" status or Available with 0 quantity, show "Out of Stock"
                Status = (f.Status.Equals("Available", StringComparison.OrdinalIgnoreCase) && f.Quantity > 0)
                    ? "In Stock"
                    : "Out of Stock",
                ImageUrl = string.IsNullOrEmpty(f.ImageUrl)
                    ? $"/images/fridges/fridge{f.FridgeId}.jpg"  // default naming based on ID
                    : f.ImageUrl
            }).ToList();

            var model = new CustomerViewModel
            {
                FullNames = User.Identity?.Name ?? "Guest",
                Fridges = fridgeViewModels
            };

            ViewData["Search"] = search;
            ViewData["StockFilter"] = stockFilter; // Pass filter to view

            return View(model);
        }

        // ==========================
        // 2. ADD TO CART (Updated - No Stock Deduction)
        // ==========================
        [HttpPost]
        public async Task<IActionResult> AddToCart(int fridgeId, int quantity = 1)
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            // Find or create the customer's cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerID = customerId,
                    IsActive = true
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            else if (!cart.IsActive)
            {
                cart.IsActive = true;
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
            }

            // Find the fridge
            var fridge = await _context.Fridge.FindAsync(fridgeId);
            if (fridge == null) return NotFound();

            // ✅ Check if there is enough stock
            if (fridge.Quantity < quantity)
            {
                TempData["Error"] = $"Not enough stock for {fridge.Brand} {fridge.Model}. Only {fridge.Quantity} left.";
                return RedirectToAction("Index", "Fridge");
            }

            // ✅ Add or update cart item
            var existingItem = cart.CartItems.FirstOrDefault(i => i.FridgeId == fridgeId && !i.IsDeleted);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Price = fridge.Price;
                _context.CartItems.Update(existingItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.CartId,
                    FridgeId = fridgeId,
                    Quantity = quantity,
                    Price = fridge.Price,
                    IsDeleted = false
                };
                _context.CartItems.Add(newCartItem);
            }

            // ✅ No fridge quantity deduction here (only check stock)
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{fridge.Brand} {fridge.Model} added to cart successfully!";
            return RedirectToAction("ViewCart");
        }


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

            // ✅ Get the fridge linked to this cart item
            var fridge = await _context.Fridge.FindAsync(item.FridgeId);
            if (fridge != null)
            {
                // ✅ Add the quantity back to the fridge stock
                fridge.Quantity += item.Quantity;

                // ✅ If fridge was out of stock and now has stock, update its status
                if (fridge.Quantity > 0 && fridge.Status == "Out of Stock")
                {
                    fridge.Status = "In Stock";
                }

                _context.Fridge.Update(fridge);
            }

            // ✅ Soft delete the cart item (keeps cart history)
            item.IsDeleted = true;
            _context.CartItems.Update(item);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Item removed from cart and stock updated.";
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
                .FirstOrDefaultAsync(c => c.CustomerID == customerId && c.IsActive);

            if (cart == null || !cart.CartItems.Any())
            {
                ViewBag.Message = "Your cart is empty.";
                return View(new Cart { CartItems = new List<CartItem>() });
            }

            return View(cart);
        }

        // ==========================
        // 4. CHECKOUT (View)
        // ==========================
        public async Task<IActionResult> Checkout()
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems.Where(ci => !ci.IsDeleted))
                .ThenInclude(i => i.Fridge)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId && c.IsActive);

            if (cart == null || !cart.CartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("ViewCart");
            }

            return View(cart);
        }

        // ==========================
        // 4. CHECKOUT - POST (Confirm Checkout & Reduce Stock)
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckout(
            string fullName,
            string phoneNumber,
            string deliveryAddress,
            DateTime? orderDate) // <-- Added optional orderDate
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems.Where(ci => !ci.IsDeleted))
                .ThenInclude(i => i.Fridge)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId && c.IsActive);

            if (cart == null || !cart.CartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("ViewCart");
            }

            // ✅ Determine which date to use: user-provided (within 6 months) or DateTime.Now
            var orderDateToUse = orderDate.HasValue && orderDate.Value >= DateTime.Now.AddMonths(-6)
                ? orderDate.Value
                : DateTime.Now;

            // ✅ Create new order
            var order = new Order
            {
                CustomerID = customerId,
                OrderDate = orderDateToUse, // <-- Use allowed past date
                Status = "Pending Payment",
                DeliveryAddress = deliveryAddress,
                ContactName = fullName,
                ContactPhone = phoneNumber,
                PaymentDate = DateTime.Now,
                TotalAmount = cart.CartItems.Sum(i => i.Price * i.Quantity),
                OrderItems = new List<OrderItem>()
            };

            // ✅ Reduce stock and add order items
            foreach (var ci in cart.CartItems)
            {
                var fridge = ci.Fridge;

                if (fridge.Quantity >= ci.Quantity)
                {
                    fridge.Quantity -= ci.Quantity;

                    if (fridge.Quantity <= 0)
                        fridge.Status = "Out of Stock";

                    _context.Fridge.Update(fridge);
                }
                else
                {
                    TempData["Error"] = $"Not enough stock for {fridge.Brand} {fridge.Model}.";
                    return RedirectToAction("ViewCart");
                }

                // ✅ Add each cart item to order items
                order.OrderItems.Add(new OrderItem
                {
                    FridgeId = ci.FridgeId,
                    Quantity = ci.Quantity,
                    Price = ci.Price
                });
            }

            // ✅ Save order and update cart
            _context.Orders.Add(order);

            // Mark cart as inactive after checkout
            cart.IsActive = false;
            _context.Carts.Update(cart);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Checkout completed successfully! Your order has been placed.";
            return RedirectToAction("AddCard", new { orderId = order.OrderId });
        }


        // ✅ DEBUG VERSION: Check for pending orders and resume checkout
        public async Task<IActionResult> ResumeCheckout()
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0)
            {
                TempData["Error"] = "Please log in first.";
                return RedirectToAction("Login", "Account");
            }

            _logger.LogInformation($"ResumeCheckout called for customer {customerId}");

            try
            {
                // Check all possible pending statuses
                var pendingOrders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Fridge)
                    .Where(o => o.CustomerID == customerId &&
                               (o.Status == "Pending Payment" ||
                                o.Status == "Pending" ||
                                o.Status == "AwaitingPayment"))
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                _logger.LogInformation($"Found {pendingOrders.Count} pending orders for customer {customerId}");

                if (pendingOrders.Any())
                {
                    var latestOrder = pendingOrders.First();
                    _logger.LogInformation($"Redirecting to payment for order {latestOrder.OrderId} with status: {latestOrder.Status}");

                    TempData["Success"] = "Found your pending order! Please complete payment.";
                    return RedirectToAction("AddCard", new { orderId = latestOrder.OrderId });
                }
                else
                {
                    // Check what orders actually exist for this customer
                    var allOrders = await _context.Orders
                        .Where(o => o.CustomerID == customerId)
                        .Select(o => new { o.OrderId, o.Status, o.OrderDate })
                        .OrderByDescending(o => o.OrderDate)
                        .ToListAsync();

                    _logger.LogInformation($"Customer {customerId} has {allOrders.Count} total orders: {string.Join(", ", allOrders.Select(o => $"Order {o.OrderId} ({o.Status})"))}");

                    if (allOrders.Any(o => o.Status == "Paid" || o.Status == "Completed"))
                    {
                        TempData["Info"] = "You don't have any pending orders. All your orders are completed or paid.";
                    }
                    else
                    {
                        TempData["Error"] = "No pending orders found. Please add items to your cart and checkout again.";
                    }

                    return RedirectToAction("ViewCart");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ResumeCheckout for customer {CustomerId}", customerId);
                TempData["Error"] = "An error occurred while checking for pending orders. Please try again.";
                return RedirectToAction("ViewCart");
            }
        }

        // ==========================
        // PAYMENT METHODS (Card + EFT Only)
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
        public async Task<IActionResult> AddCard(PaymentViewModel model, DateTime? paymentDate) // <-- optional paymentDate
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.OrderId == model.OrderId && o.CustomerID == customerId);

            if (order == null) return Forbid();

            // Recompute amount server-side for safety
            model.Amount = await _context.OrderItems
                .Where(oi => oi.OrderId == model.OrderId)
                .SumAsync(oi => oi.Price * oi.Quantity);

            if (!ModelState.IsValid)
            {
                model.Orders = order;
                return View(model);
            }

            // ✅ Allow past payment date up to 6 months
            var paymentDateToUse = paymentDate.HasValue && paymentDate.Value >= DateTime.Now.AddMonths(-6)
                ? paymentDate.Value
                : DateTime.Now;

            Payment payment = new Payment
            {
                OrderId = model.OrderId,
                Amount = model.Amount,
                Method = model.Method,
                PaymentDate = paymentDateToUse // <-- updated
            };

            if (model.Method == Method.Card)
            {
                payment.CardNumber = MaskCardNumber(model.CardNumber);
                payment.Status = "Paid";
                order.Status = "Paid";

                await CartHelper.ClearCartAsync(_context, customerId);
            }
            else if (model.Method == Method.EFT)
            {
                payment.PaymentReference = SerialNumberGenerator.GeneratePaymentReference();
                payment.BankReference = model.BankReference;

                if (model.ProofOfPayment != null && model.ProofOfPayment.Length > 0)
                {
                    var fileName = $"{payment.PaymentReference}_{model.ProofOfPayment.FileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/payments", fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await model.ProofOfPayment.CopyToAsync(stream);

                    payment.ProofFilePath = "/uploads/payments/" + fileName;
                    payment.Status = "AwaitingAdminApproval";
                    order.Status = "AwaitingPayment";
                }
                else
                {
                    payment.Status = "Pending";
                    order.Status = "AwaitingPayment";
                }
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var admins = await _userManager.GetUsersInRoleAsync(Roles.Admin);
            var admin = admins.FirstOrDefault();
            if (admin != null)
            {
                await _notificationService.CreateAsync(admin.Id,
                    $"A new EFT payment (Order #{order.OrderId}) is awaiting your approval.",
                    Url.Action("PendingPayments", "ManageCustomer", new { area = "Administrator" }));
            }

            return RedirectToAction("PaymentConfirmation", new { orderId = order.OrderId });
        }

        // ✅ NEW METHOD: Clear cart only after successful payment
        private async Task ClearCartAfterSuccessfulPayment(int customerId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId && c.IsActive);

            if (cart != null)
            {
                // Mark all cart items as deleted
                foreach (var item in cart.CartItems.Where(ci => !ci.IsDeleted))
                {
                    item.IsDeleted = true;
                }

                // Mark cart as inactive
                cart.IsActive = false;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> PaymentConfirmation(int orderId)
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Fridge)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerID == customerId);

            if (order == null) return NotFound();

            await _context.Entry(order).Collection(o => o.Payments).LoadAsync();

            var payment = order.Payments
                .OrderByDescending(p => p.PaymentDate)
                .FirstOrDefault();

            ViewBag.Payment = payment ?? new Payment { Status = order.Status };

            return View(order);
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
                .Include(o => o.Payments)
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

            // Load the order with its items and the associated fridge
            var order = await _context.Orders
                .Where(o => o.OrderId == id && o.CustomerID == customer.CustomerID)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Fridge)
                .Include(o => o.OrderItems) // Include allocations directly
                    .ThenInclude(oi => oi.Fridge.FridgeAllocation)
                .FirstOrDefaultAsync();

            if (order == null)
                return NotFound("Order not found.");

            // For each order item, find the specific allocation
            foreach (var item in order.OrderItems)
            {
                var allocation = item.Fridge?.FridgeAllocation?
                    .FirstOrDefault(a => a.CustomerID == customer.CustomerID &&
                                       a.OrderItemId == item.OrderItemId);

                if (allocation != null)
                {
                    item.AllocationID = allocation.AllocationID;
                    item.AllocationStatus = allocation.Status;
                    item.ReturnRequested = allocation.ReturnRequested;
                }
            }

            return View(order);
        }


        [Authorize]
        public async Task<IActionResult> RequestReturn(int allocationId)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.ApplicationUserId == appUser.Id);

            if (customer == null)
                return NotFound("Customer profile not found.");

            var allocation = await _context.FridgeAllocation
                .Include(a => a.Fridge)
                .FirstOrDefaultAsync(a => a.AllocationID == allocationId && a.CustomerID == customer.CustomerID);

            if (allocation == null || allocation.Status != "Fridge Allocated")
                return NotFound();

            allocation.ReturnRequested = true;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Return request submitted successfully!";
            return RedirectToAction("ViewOrder", new { id = allocation.OrderItemId });
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
                .Include(o => o.Payments)
                .AsNoTracking()// ✅ include payments
                .ToListAsync();

            // Update order.Status based on latest payment
            foreach (var order in orders)
            {
                var latestPayment = await _context.Payments
             .Where(p => p.OrderId == order.OrderId)
             .OrderByDescending(p => p.PaymentDate)
             .FirstOrDefaultAsync();

                if (latestPayment != null)
                {
                    order.Status = latestPayment.Status switch
                    {
                        "Paid" => "Paid", // ✅ show "Paid" instead of "Completed"
                        "AwaitingAdminApproval" => "Awaiting Approval",
                        "Pending" => "Awaiting Payment",
                        "Rejected" => "Rejected",
                        _ => order.Status
                    };
                }
            }

            return View(orders);
        }

        public async Task<IActionResult> TrackOrder(int? orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Fridge) // ✅ Load the Fridge for each order item
                        .ThenInclude(f => f.FridgeAllocation) // ✅ Also load the FridgeAllocation
                .Include(o => o.Payments)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync(o => !orderId.HasValue || o.OrderId == orderId);

            if (order == null)
                return NotFound();

            // Set progress based on status
            switch (order.Status)
            {
                case "Paid":
                    order.OrderProgress = OrderStatus.OrderPlaced;
                    break;
                case "Fridge Allocated":
                    order.OrderProgress = OrderStatus.FridgeAllocated;
                    break;
                case "Shipped":
                    order.OrderProgress = OrderStatus.Shipped;
                    break;
                case "Delivered":
                    order.OrderProgress = OrderStatus.Delivered;
                    break;
                default:
                    order.OrderProgress = OrderStatus.OrderPlaced;
                    break;
            }

            await _context.SaveChangesAsync();

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Fridge)
                .FirstOrDefaultAsync(o => o.OrderId == id && o.CustomerID == customerId);

            if (order == null) return NotFound();

            return View(order);
        }

        // ==========================
        // 8. FAULT MANAGEMENT (SIMPLIFIED)
        // ==========================
        public async Task<IActionResult> MyFaults()
        {
            try
            {
                var customerId = GetLoggedInCustomerId();

                var faultReports = await _context.FaultReport
                    .Include(fr => fr.Fridge)
                    .Where(fr => fr.Fridge.CustomerID == customerId)
                    .OrderByDescending(fr => fr.ReportDate)
                    .ToListAsync();

                return View(faultReports);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error loading faults.";
                return View(new List<FaultReport>());
            }
        }

        public async Task<IActionResult> FaultDetails(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var customerId = GetLoggedInCustomerId();

                var faultReport = await _context.FaultReport
                    .Include(fr => fr.Fridge)
                    .FirstOrDefaultAsync(fr => fr.FaultReportId == id &&
                                             fr.Fridge.CustomerID == customerId);

                return faultReport == null ? NotFound() : View(faultReport);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error loading fault details.";
                return RedirectToAction(nameof(MyFaults));
            }
        }

        // MODIFIED: Added orderId parameter to pre-select fridges from specific order
        [HttpGet]
        public async Task<IActionResult> CreateFault(int? fridgeId, int? orderId)
        {
            var customerId = GetLoggedInCustomerId();
            var viewModel = new CreateFaultViewModel
            {
                FridgeOptions = await GetCustomerFridgesAsync(),
                PriorityOptions = GetPriorityOptions()
            };

            // If fridgeId is provided AND belongs to customer
            if (fridgeId.HasValue)
            {
                var selectedFridge = await _context.Fridge
                    .FirstOrDefaultAsync(f => f.FridgeId == fridgeId.Value && f.CustomerID == customerId);

                if (selectedFridge != null)
                {
                    viewModel.FridgeId = fridgeId.Value;
                    ViewBag.HasPreselectedFridge = true;
                    ViewBag.SelectedFridgeInfo = $"{selectedFridge.Brand} {selectedFridge.Model} - {selectedFridge.FridgeType}";

                    // Optional: Add order context if orderId is provided
                    if (orderId.HasValue)
                    {
                        ViewBag.OrderContext = $" (from Order #{orderId.Value})";
                    }
                }
                else
                {
                    // Fridge doesn't belong to customer or doesn't exist
                    ViewBag.HasPreselectedFridge = false;
                    TempData["WarningMessage"] = "The selected fridge was not found in your account.";
                }
            }
            else
            {
                ViewBag.HasPreselectedFridge = false;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFault(CreateFaultViewModel viewModel, DateTime? reportDate) // <-- added optional reportDate
        {
            if (!ModelState.IsValid)
            {
                viewModel.FridgeOptions = await GetCustomerFridgesAsync();
                viewModel.PriorityOptions = GetPriorityOptions();
                return View(viewModel);
            }

            try
            {
                var customerId = GetLoggedInCustomerId();
                var fridge = await _context.Fridge
                    .FirstOrDefaultAsync(f => f.FridgeId == viewModel.FridgeId && f.CustomerID == customerId);

                if (fridge == null)
                {
                    ModelState.AddModelError("FridgeId", "Selected fridge not found or doesn't belong to you.");
                    viewModel.FridgeOptions = await GetCustomerFridgesAsync();
                    viewModel.PriorityOptions = GetPriorityOptions();
                    return View(viewModel);
                }

                // ✅ Allow past date up to 6 months, otherwise use now
                var reportDateToUse = reportDate.HasValue && reportDate.Value >= DateTime.Now.AddMonths(-6)
                    ? reportDate.Value
                    : DateTime.Now;

                var faultReport = new FaultReport
                {
                    FridgeId = viewModel.FridgeId,
                    FaultDescription = viewModel.FaultDescription,
                    FaultType = viewModel.FaultType,
                    ReportDate = reportDateToUse, // <-- updated
                    UrgencyLevel = MapPriorityToUrgency(viewModel.Priority),
                    Status = TaskStatus.Pending,
                };

                _context.FaultReport.Add(faultReport);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fault reported successfully! Our technicians will review it shortly.";
                return RedirectToAction(nameof(FaultDetails), new { id = faultReport.FaultReportId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating fault report");
                viewModel.FridgeOptions = await GetCustomerFridgesAsync();
                viewModel.PriorityOptions = GetPriorityOptions();
                TempData["ErrorMessage"] = "Error reporting fault. Please try again.";
                return View(viewModel);
            }
        }


        // NEW HELPER METHOD: Get fridges from a specific order
        private async Task<List<SelectListItem>> GetFridgesFromOrderAsync(int orderId, int customerId)
        {
            return await _context.Orders
                .Where(o => o.OrderId == orderId && o.CustomerID == customerId)
                .SelectMany(o => o.OrderItems)
                .Select(oi => oi.Fridge)
                .Where(f => f.Status == "Active")
                .Select(f => new SelectListItem
                {
                    Value = f.FridgeId.ToString(),
                    Text = $"{f.Brand} {f.Model} - {f.SerialNumber} (Order #{orderId})"
                })
                .Distinct()
                .ToListAsync();
        }

        // Helper method
        private UrgencyLevel MapPriorityToUrgency(string priority)
        {
            return priority switch
            {
                "Critical" => UrgencyLevel.Critical,
                "High" => UrgencyLevel.High,
                "Medium" => UrgencyLevel.Medium,
                "Low" => UrgencyLevel.Low,
                _ => UrgencyLevel.Medium
            };
        }

        // Add these small helper methods
        private async Task<List<SelectListItem>> GetCustomerFridgesAsync()
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return new List<SelectListItem>();

            return await _context.Fridge
                .Where(f => f.CustomerID == customerId &&
                           (f.Status == "Active" || f.Status == "Allocated"))
                .Select(f => new SelectListItem
                {
                    Value = f.FridgeId.ToString(),
                    Text = $"{f.Brand} {f.Model} - {f.FridgeType}"
                })
                .ToListAsync();
        }

        private List<SelectListItem> GetPriorityOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Low", Text = "Low" },
                new SelectListItem { Value = "Medium", Text = "Medium" },
                new SelectListItem { Value = "High", Text = "High" },
                new SelectListItem { Value = "Critical", Text = "Critical" }
            };
        }

        // Cancel Fault (if allowed)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelFault(int id)
        {
            try
            {
                var customerId = GetLoggedInCustomerId();

                var faultReport = await _context.FaultReport
                    .Include(fr => fr.Fridge)
                    .FirstOrDefaultAsync(fr => fr.FaultReportId == id &&
                                             fr.Fridge.CustomerID == customerId);

                if (faultReport == null)
                {
                    TempData["ErrorMessage"] = "Fault report not found.";
                    return RedirectToAction(nameof(MyFaults));
                }

                // Check if it's still cancellable (only pending reports)
                if (faultReport.Status != TaskStatus.Pending)
                {
                    TempData["ErrorMessage"] = "Only pending fault reports can be cancelled.";
                    return RedirectToAction(nameof(FaultDetails), new { id });
                }

                // Update status
                faultReport.Status = TaskStatus.Cancelled;
                _context.FaultReport.Update(faultReport);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fault report cancelled successfully.";
                return RedirectToAction(nameof(MyFaults));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling fault report");
                TempData["ErrorMessage"] = "Error cancelling fault report.";
                return RedirectToAction(nameof(FaultDetails), new { id });
            }
        }

        // Update Fault Description
        public async Task<IActionResult> UpdateFaultDescription(int id, string description)
        {
            try
            {
                var customerId = GetLoggedInCustomerId();

                var faultReport = await _context.FaultReport
                    .Include(fr => fr.Fridge)
                    .FirstOrDefaultAsync(fr => fr.FaultReportId == id &&
                                             fr.Fridge.CustomerID == customerId);

                if (faultReport == null)
                {
                    return Json(new { success = false, message = "Fault report not found." });
                }

                if (faultReport.Status != TaskStatus.Pending)
                {
                    return Json(new { success = false, message = "Only pending fault reports can be updated." });
                }

                faultReport.FaultDescription = description;
                _context.FaultReport.Update(faultReport);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Fault description updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating fault description");
                return Json(new { success = false, message = "Error updating fault description." });
            }
        }

        public async Task<IActionResult> FaultStatus(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var customerId = GetLoggedInCustomerId();

                var faultReport = await _context.FaultReport
                    .Include(fr => fr.Fridge)
                    .FirstOrDefaultAsync(fr => fr.FaultReportId == id &&
                                             fr.Fridge.CustomerID == customerId);

                return faultReport == null ? NotFound() : View(faultReport);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error loading fault status.";
                return RedirectToAction(nameof(MyFaults));
            }
        }

        // ==========================
        // 9. SERVICE HISTORY
        // ==========================
        public async Task<IActionResult> UpcomingVisits()
        {
            var customerId = GetLoggedInCustomerId();
            var visits = await _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                .ThenInclude(r => r.Fridge)
                .Include(v => v.Employee)
                .Where(v => v.MaintenanceRequest.Fridge.CustomerID == customerId &&
                            (v.Status == TaskStatus.Scheduled || v.Status == TaskStatus.Rescheduled || v.MaintenanceRequest.TaskStatus == TaskStatus.Scheduled || v.MaintenanceRequest.TaskStatus == TaskStatus.Rescheduled || v.Status == TaskStatus.Cancelled || v.MaintenanceRequest.TaskStatus == TaskStatus.Cancelled))
                .OrderBy(v => v.ScheduledDate)
                .ThenBy(v => v.ScheduledTime)
                .ToListAsync();

            return View(visits);
        }

        public async Task<IActionResult> FridgeServiceHistory(int fridgeId)
        {
            var visits = await _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                    .ThenInclude(m => m.Fridge)
                .Include(v => v.MaintenanceChecklist)
                .Include(v => v.ComponentUsed)
                .Include(v => v.FaultReport)
                .Where(v =>
                v.MaintenanceRequest.FridgeId == fridgeId &&
                v.Status == TaskStatus.Complete &&
                v.MaintenanceRequest.CompletedDate != null
)
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
            if (userIdClaim == null)
            {
                _logger.LogWarning("No user ID claim found");
                return 0;
            }

            if (!int.TryParse(userIdClaim.Value, out var appUserId))
            {
                _logger.LogWarning($"Failed to parse user ID: {userIdClaim.Value}");
                return 0;
            }

            var customer = _context.Customers.FirstOrDefault(c => c.ApplicationUserId == appUserId);
            if (customer == null)
            {
                _logger.LogWarning($"No customer found for application user ID: {appUserId}");
                return 0;
            }

            _logger.LogInformation($"Found customer ID: {customer.CustomerID}");
            return customer.CustomerID;
        }

        // ✅ ADD THIS MISSING METHOD
        private async Task<int> GetCurrentApplicationUserIdAsync()
        {
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser == null) return 0;
            return appUser.Id;
        }

        private string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber)) return null;
            var cleaned = cardNumber.Replace(" ", "");
            var last4 = cleaned.Length >= 4 ? cleaned[^4..] : cleaned;
            return new string('*', Math.Max(0, cleaned.Length - 4)) + last4;
        }

        public async Task<IActionResult> Notifications()
        {
            var appUserId = await GetCurrentApplicationUserIdAsync();
            if (appUserId == 0) return RedirectToAction("Login", "Account");

            var notifications = await _notificationService.GetAllAsync(appUserId);
            return View(notifications);
        }

        [HttpGet]
        public async Task<IActionResult> Unread()
        {
            var appUserId = await GetCurrentApplicationUserIdAsync();
            if (appUserId == 0) return Json(new { });

            var notifications = await _notificationService.GetUnreadAsync(appUserId, 5);
            return Json(notifications.Select(n => new { n.NotificationId, n.Message, n.Url }));
        }

        [HttpGet]
        public async Task<IActionResult> UnreadCount()
        {
            var appUserId = await GetCurrentApplicationUserIdAsync();
            if (appUserId == 0) return Json(0);

            var count = (await _notificationService.GetUnreadAsync(appUserId)).Count();
            return Json(count);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var appUserId = await GetCurrentApplicationUserIdAsync();
            if (appUserId == 0) return RedirectToAction("Login", "Account");

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null || notification.UserId != appUserId)
            {
                return Forbid();
            }

            await _notificationService.MarkAsReadAsync(id);
            return RedirectToAction("Notifications");
        }
    }
}