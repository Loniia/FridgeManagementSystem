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
        private readonly ILogger<CustomerController> _logger;

        // Constructor
        public CustomerController(
            FridgeDbContext context,
            FridgeService fridgeService,
            UserManager<ApplicationUser> userManager,
            ILogger<CustomerController> logger)
        {
            _context = context;
            _fridgeService = fridgeService;
            _userManager = userManager;
            _logger = logger;
        }

        // ==========================
        // 1. DASHBOARD
        // ==========================
        public async Task<IActionResult> Dashboard(string search)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return RedirectToAction("Login", "Account");

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            var fridgesQuery = _context.Fridge
                .Include(f => f.FridgeAllocation)
                .AsQueryable();

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

            var existingItem = cart.CartItems.FirstOrDefault(i => i.FridgeId == fridgeId);
            if (existingItem != null)
            {
                if (existingItem.IsDeleted)
                {
                    existingItem.IsDeleted = false;
                    existingItem.Quantity = quantity;
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

            cart.IsActive = true;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckout(string fullName, string phoneNumber, string deliveryAddress)
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

            var order = new Order
            {
                CustomerID = customerId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                DeliveryAddress = deliveryAddress,
                ContactName = fullName,
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
            cart.IsActive = false;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("AddCard", new { orderId = order.OrderId });
        }

        // ==========================
        // 5. PAYMENT METHODS
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
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == model.OrderId && o.CustomerID == customerId);

            if (order == null) return Forbid();

            model.Amount = await _context.OrderItems
                .Where(oi => oi.OrderId == model.OrderId)
                .SumAsync(oi => oi.Price * oi.Quantity);

            if (!ModelState.IsValid)
            {
                model.Orders = order;
                return View(model);
            }

            Payment payment = new Payment
            {
                OrderId = model.OrderId,
                Amount = model.Amount,
                Method = model.Method,
                PaymentDate = DateTime.Now
            };

            if (model.Method == Method.Card)
            {
                payment.CardNumber = MaskCardNumber(model.CardNumber);
                payment.Status = "Paid";
                order.Status = "Paid";
            }
            else if (model.Method == Method.EFT)
            {
                payment.PaymentReference = GeneratePaymentReference();
                payment.BankReference = model.BankReference;
                payment.Status = "Pending";

                if (model.ProofOfPayment != null && model.ProofOfPayment.Length > 0)
                {
                    var fileName = $"{payment.PaymentReference}_{model.ProofOfPayment.FileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/payments", fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await model.ProofOfPayment.CopyToAsync(stream);

                    payment.ProofFilePath = "/uploads/payments/" + fileName;
                    payment.Status = "AwaitingAdminApproval";
                }

                order.Status = "AwaitingPayment";
            }
            else if (model.Method == Method.PayPal)
            {
                payment.Status = "Pending";
                order.Status = "AwaitingPayment";
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return RedirectToAction("PaymentConfirmation", new { orderId = order.OrderId });
        }

        // ==========================
        // 6. PAYMENT CONFIRMATION
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

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("MyAccount");
            }

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
            if (!ModelState.IsValid)
            {
                // ADD THESE 2 LINES - Repopulate dropdowns
                viewModel.FridgeOptions = await GetCustomerFridgesAsync();
                viewModel.PriorityOptions = GetPriorityOptions();
                return View(viewModel);
            }

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
                // ADD THESE 2 LINES - Repopulate dropdowns on error too
                viewModel.FridgeOptions = await GetCustomerFridgesAsync();
                viewModel.PriorityOptions = GetPriorityOptions();
                TempData["ErrorMessage"] = "Error reporting fault.";
                return View(viewModel);
            }
        }

        // Add these small helper methods
        private async Task<List<SelectListItem>> GetCustomerFridgesAsync()
        {
            var customerId = GetLoggedInCustomerId();
            return await _context.Fridge
                .Where(f => f.CustomerID == customerId && f.Status == "Active")
                .Select(f => new SelectListItem
                {
                    Value = f.FridgeId.ToString(),
                    Text = $"{f.Brand} {f.Model} - {f.SerialNumber}"
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
                var fault = await _context.Faults
                    .FirstOrDefaultAsync(f => f.FaultID == id && f.CustomerId == customerId);

                if (fault == null)
                {
                    TempData["ErrorMessage"] = "Fault not found.";
                    return RedirectToAction(nameof(MyFaults));
                }

                if (fault.Status != "Pending")
                {
                    TempData["ErrorMessage"] = "Only pending faults can be cancelled.";
                    return RedirectToAction(nameof(FaultDetails), new { id });
                }

                fault.Status = "Cancelled";

                _context.Update(fault);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fault cancelled successfully.";
                return RedirectToAction(nameof(MyFaults));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling fault");
                TempData["ErrorMessage"] = "Error cancelling fault.";
                return RedirectToAction(nameof(FaultDetails), new { id });
            }
        }

        // Update Fault (if description needs updating)
        public async Task<IActionResult> UpdateFaultDescription(int id, string description)
        {
            try
            {
                var customerId = GetLoggedInCustomerId();
                var fault = await _context.Faults
                    .FirstOrDefaultAsync(f => f.FaultID == id && f.CustomerId == customerId);

                if (fault == null)
                {
                    return Json(new { success = false, message = "Fault not found." });
                }

                if (fault.Status != "Pending")
                {
                    return Json(new { success = false, message = "Only pending faults can be updated." });
                }

                fault.FaultDescription = description;
                _context.Update(fault);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Fault description updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating fault description");
                return Json(new { success = false, message = "Error updating fault description." });
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

        private string GeneratePaymentReference()
        {
            return "PAY-" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        }
    }
}