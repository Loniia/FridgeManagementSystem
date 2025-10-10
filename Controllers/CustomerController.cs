using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

        public CustomerController(FridgeDbContext context, FridgeService fridgeService)
        {
            _context = context;
            _fridgeService = fridgeService;
        }

        // ==========================
        // 1. DASHBOARD
        // ==========================
        public async Task<IActionResult> Dashboard()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            var fridges = await _context.Fridge
                .Include(f => f.FridgeAllocation)
                .ToListAsync();

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

            var cart = await _context.Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

            if (cart == null)
            {
                cart = new Cart { CustomerID = customerId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var fridge = await _context.Fridge.FindAsync(fridgeId);
            if (fridge == null) return NotFound();

            var existingItem = cart.CartItems.FirstOrDefault(i => i.FridgeId == fridgeId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Price = fridge.Price;
            }
            else
            {
                cart.CartItems.Add(new CartItem { FridgeId = fridgeId, Quantity = quantity, Price = fridge.Price });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ViewCart");
        }

        // ==========================
        // 3. VIEW CART
        // ==========================
        public async Task<IActionResult> ViewCart()
        {
            var customerId = GetLoggedInCustomerId();

            var cart = await _context.Carts
                .Include(c => c.CartItems.Where(ci => !ci.IsDeleted)) // only active items
                .ThenInclude(ci => ci.Fridge)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

            return View(cart);
        }

        // ==========================
        // 4. CHECKOUT
        // ==========================
        public async Task<IActionResult> Checkout()
        {
            var customerId = GetLoggedInCustomerId();
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Fridge)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckout(string deliveryAddress)
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
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

            // Soft delete instead of removing
            cart.IsActive = false;
            _context.Carts.Update(cart);

            await _context.SaveChangesAsync();

            return RedirectToAction("AddCard", new { orderId = order.OrderId, amount = order.TotalAmount });
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
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerID == customerId);

            if (order == null) return NotFound();

            var vm = new PaymentViewModel
            {
                OrderId = order.OrderId,
                Amount = amount ?? order.TotalAmount,
                Method = Method.Card
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCard(PaymentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var customerId = GetLoggedInCustomerId();
            var order = await _context.Orders.FindAsync(model.OrderId);

            if (order == null || order.CustomerID != customerId) return Forbid();

            model.Amount = await _context.OrderItems
                .Where(oi => oi.OrderId == model.OrderId)
                .SumAsync(oi => oi.Price * oi.Quantity);

            var payment = new Payment
            {
                OrderId = model.OrderId,
                Amount = model.Amount,
                Method = model.Method,
                CardNumber = MaskCardNumber(model.CardNumber),
                BankReference = model.BankReference,
                PaymentDate = DateTime.Now,
                Status = "Paid"
            };

            _context.Payments.Add(payment);
            order.Status = "Paid";
            await _context.SaveChangesAsync();

            return RedirectToAction("PaymentConfirmation", new { id = payment.PaymentId });
        }

        public IActionResult PaymentConfirmation() => View();

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
        // 9. REQUEST MANAGEMENT
        // ==========================
        //public IActionResult CreateRequest() => View();

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateRequest([Bind("RequiredModel,Quantity,RequiredDate,SpecialRequirements")] FridgeRequest request)
        //{
        //    if (!ModelState.IsValid) return View(request);

        //    request.CustomerId = GetLoggedInCustomerId();
        //    request.Status = "Pending";
        //    request.RequestDate = DateTime.Now;
        //    request.RequestCode = GenerateRequestCode();

        //    _context.FridgeRequests.Add(request);
        //    await _context.SaveChangesAsync();

        //    TempData["SuccessMessage"] = "Fridge request submitted.";
        //    return RedirectToAction(nameof(RequestDetails), new { id = request.RequestId });
        //}

        //public async Task<IActionResult> MyRequests()
        //{
        //    var customerId = GetLoggedInCustomerId();
        //    //var requests = await _context.FridgeRequests
        //        .Where(r => r.CustomerId == customerId)
        //        .OrderByDescending(r => r.RequestId)
        //        .ToListAsync();

        //    return View(requests);
        //}

        //public async Task<IActionResult> RequestDetails(int? id)
        //{
        //    if (id == null) return NotFound();

        //    var customerId = GetLoggedInCustomerId();
        //    //var request = await _context.FridgeRequests
        //        .FirstOrDefaultAsync(r => r.RequestId == id && r.CustomerId == customerId);

        //    return request == null ? NotFound() : View(request);
        //}

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

            var appUserId = int.Parse(userIdClaim.Value);
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