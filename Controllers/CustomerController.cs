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

        public CustomerController(FridgeDbContext context)
        {
            _context = context;
        }

        // ==========================
        // 1. DASHBOARD
        // ==========================
        public async Task<IActionResult> Dashboard()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            // Get available fridges
            var availableFridges = await _context.Fridge
                .Include(f => f.FridgeAllocation)
                .Where(f => f.Status == "Available" || f.Status == "Received")
                .ToListAsync();

            var fridgeViewModels = availableFridges.Select(f => new FridgeViewModel
            {
                FridgeId = f.FridgeId,
                FridgeType = f.FridgeType,
                Brand = f.Brand,
                Model = f.Model,
                Price = f.Price,
                Quantity = f.Quantity,
                ImageUrl = string.IsNullOrEmpty(f.ImageUrl) ? "/images/fridges/default.jpg" : f.ImageUrl,
                AvailableStock = f.Quantity - (f.FridgeAllocation?.Count(a =>
                    a.ReturnDate == null || a.ReturnDate > DateOnly.FromDateTime(DateTime.Today)) ?? 0)
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
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Fridge)
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
        public async Task<IActionResult> ConfirmCheckout()
        {
            var customerId = GetLoggedInCustomerId();
            if (customerId == 0) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Fridge)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

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
            _context.Carts.Remove(cart);
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

    // GET: CustomerFault/CreateRequest - Request a new fridge
    public IActionResult CreateRequest()
    {
        return View();
    }

    // POST: CustomerFault/CreateRequest
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRequest([Bind("RequiredModel,Quantity,RequiredDate,SpecialRequirements")] FridgeRequest request)
    {
        try
        {
            if (ModelState.IsValid)
            {
                // Set additional properties
                request.CustomerId = GetCurrentCustomerId();
                request.Status = "Pending";
                request.RequestDate = DateTime.Now;
                request.RequestCode = GenerateRequestCode();

                _context.FridgeRequests.Add(request);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fridge request submitted successfully! We'll contact you soon.";
                return RedirectToAction(nameof(RequestDetails), new { id = request.RequestId });
            }

            return View(request);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while submitting the request. Please try again.";
            return View(request);
        }
    }

    // GET: CustomerFault/MyRequests - View customer's fridge requests
    public async Task<IActionResult> MyRequests()
    {
        try
        {
            var customerId = GetCurrentCustomerId();
            var requests = await _context.FridgeRequests
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.RequestId)
                .ToListAsync();

            return View(requests);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while loading your requests.";
            return View(new List<FridgeRequest>());
        }
    }

    // GET: CustomerFault/RequestDetails/5 - View specific request details
    public async Task<IActionResult> RequestDetails(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var customerId = GetCurrentCustomerId();
            var request = await _context.FridgeRequests
                .FirstOrDefaultAsync(r => r.RequestId == id && r.CustomerId == customerId);

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while loading request details.";
            return RedirectToAction(nameof(MyRequests));
        }
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

    // GET: CustomerFault/GetRequestStatus - AJAX endpoint for request status
    [HttpGet]
    public async Task<JsonResult> GetRequestStatus(int requestId)
    {
        try
        {
            var customerId = GetCurrentCustomerId();
            var request = await _context.FridgeRequests
                .Where(r => r.RequestId == requestId && r.CustomerId == customerId)
                .Select(r => new { r.Status, r.RequiredModel, r.Quantity })
                .FirstOrDefaultAsync();

            if (request == null)
            {
                return Json(new { success = false, message = "Request not found" });
            }

            return Json(new { success = true, status = request.Status, model = request.RequiredModel, quantity = request.Quantity });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "Error retrieving request status" });
        }
    }
    // Helper Methods
    private int GetCurrentCustomerId()
    {
        // Implement this based on your authentication system
        // This is a placeholder - you'll need to get the actual customer ID from your auth system
        // For now, returning 1 as example
        return 1;

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














//using FridgeManagementSystem.Data;
//using FridgeManagementSystem.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//#nullable disable

//namespace FridgeManagementSystem.Controllers
//{
//    [Authorize(Roles = "Customer")]
//        public class CustomerController : Controller
//        {
//            private readonly FridgeDbContext _context;

//            public CustomerController(FridgeDbContext context)
//            {
//                _context = context;
//            }

//        //Dashboard
//        public IActionResult Dashboard()
//        {
//            return View();
//        }

//        // GET: Customer
//        public async Task<IActionResult> Index()
//            {
//                var customers = await _context.Customers
//                    .Include(c => c.UserAccount)
//                    .ToListAsync();
//                return View(customers);
//            }

//            // GET: Customer/Details/5
//            public async Task<IActionResult> Details(int? id)
//            {
//                if (id == null) return NotFound();

//                var customer = await _context.Customers
//                    .Include(c => c.UserAccount)
//                    .FirstOrDefaultAsync(m => m.CustomerID == id);

//                if (customer == null) return NotFound();

//                return View(customer);
//            }

//            // GET: Customer/Create
//            public IActionResult Create()
//            {
//                return View();
//            }

//            // POST: Customer/Create
//            [HttpPost]
//            [ValidateAntiForgeryToken]
//            public async Task<IActionResult> Create(Customer customer)
//            {
//                if (ModelState.IsValid)
//                {
//                    _context.Add(customer);
//                    await _context.SaveChangesAsync();
//                    return RedirectToAction(nameof(Index));
//                }
//                return View(customer);
//            }

//            // GET: Customer/Edit/5
//            public async Task<IActionResult> Edit(int? id)
//            {
//                if (id == null) return NotFound();

//                var customer = await _context.Customers.FindAsync(id);
//                if (customer == null) return NotFound();

//                return View(customer);
//            }

//            // POST: Customer/Edit/5
//            [HttpPost]
//            [ValidateAntiForgeryToken]
//            public async Task<IActionResult> Edit(int id, Customer customer)
//            {
//                if (id != customer.CustomerID) return NotFound();

//                if (ModelState.IsValid)
//                {
//                    try
//                    {
//                        _context.Update(customer);
//                        await _context.SaveChangesAsync();
//                    }
//                    catch (DbUpdateConcurrencyException)
//                    {
//                        if (!_context.Customers.Any(e => e.CustomerID == id))
//                            return NotFound();
//                        else
//                            throw;
//                    }
//                    return RedirectToAction(nameof(Index));
//                }
//                return View(customer);
//            }

//            // GET: Customer/Delete/5
//            public async Task<IActionResult> Delete(int? id)
//            {
//                if (id == null) return NotFound();

//                var customer = await _context.Customers
//                    .FirstOrDefaultAsync(m => m.CustomerID == id);

//                if (customer == null) return NotFound();

//                return View(customer);
//            }

//            // POST: Customer/Delete/5
//            [HttpPost, ActionName("Delete")]
//            [ValidateAntiForgeryToken]
//            public async Task<IActionResult> DeleteConfirmed(int id)
//            {
//                var customer = await _context.Customers.FindAsync(id);
//                if (customer != null)
//                {
//                    _context.Customers.Remove(customer);
//                    await _context.SaveChangesAsync();
//                }
//                return RedirectToAction(nameof(Index));
//            }

//        // Receive & Track Fridges
//        public IActionResult ReceiveTrack()
//        {
//            var fridges = _context.Fridge
//                .ToList(); 
//            return View(fridges);
//        }
//        // customer views service history 
//        public IActionResult MyServiceHistory(int customerId)
//        {
//            var visits = _context.MaintenanceVisit
//                .Include(v => v.Fridge)
//                .Include(v => v.MaintenanceChecklist)
//                .Include(v => v.ComponentUsed)
//                .Include(v => v.FaultReport)
//                .Where(v => v.Fridge.CustomerId == customerId)
//                .OrderByDescending(v => v.ScheduledDate)
//                .ToList();

//            return View(visits);
//        }
//        // customer views upcoming maintenance visits
//        public IActionResult MyUpcomingVisits(int customerId)
//        {
//            var visits = _context.MaintenanceVisit
//                .Include(v => v.Fridge)
//                .Where(v => v.Fridge.CustomerId == customerId &&
//                            (v.Status == Models.TaskStatus.Scheduled || v.Status == Models.TaskStatus.Rescheduled))
//                .OrderBy(v => v.ScheduledDate)
//                .ToList();

//            return View(visits);
//        }

//// Create Faults
//public IActionResult CreateFault()
//{
//    return View();
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public IActionResult CreateFault(Fault model)
//{
//    if (ModelState.IsValid)
//    {
//        _context.Faults.Add(model);
//        _context.SaveChanges();
//        return RedirectToAction("Dashboard");
//    }
//    return View(model);
//}

//// Maintenance Visit
//public IActionResult MaintenanceVisit()
//{
//    var visits = _context.MaintenanceVisits.ToList(); // example
//    return View(visits);
//}

//// View Fridge History
//public IActionResult FridgeHistory()
//{
//    var history = _context.FridgeHistories.ToList(); // example
//    return View(history);
//}

//// Create Fridge Request
//public IActionResult CreateFridgeRequest()
//{
//    return View();
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public IActionResult CreateFridgeRequest(FridgeRequest model)
//{
//    if (ModelState.IsValid)
//    {
//        _context.FridgeRequests.Add(model);
//        _context.SaveChanges();
//        return RedirectToAction("Dashboard");
//    }
//    return View(model);
//}
