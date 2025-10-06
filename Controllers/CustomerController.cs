using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.ViewModels;
using System.Security.Claims;
using QuestPDF.Fluent;

public class CustomerController : Controller
{
    private readonly FridgeDbContext _context;

    public CustomerController(FridgeDbContext context)
    {
        _context = context;
    }

    //DASHBOARD
    public async Task<IActionResult> Dashboard()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Get all categories and their products
        var categories = await _context.Categories
            .Include(c => c.Products)
            .ToListAsync();

        // Get all available fridges with their allocations
        var availableFridges = await _context.Fridge
            .Include(f => f.FridgeAllocation)
            .Where(f => f.Status == "Available" || f.Status == "Received")
            .ToListAsync();

        var categoriesVm = categories.Select(c => new CategoryViewModel
        {
            Category = c,
            Products = c.Products.Select(p => new ProductViewModel
            {
                Product = p,
                AvailableStock = availableFridges
                    .Where(f =>
                        // Match product name with fridge model/brand
                        !string.IsNullOrEmpty(f.Model) &&
                        !string.IsNullOrEmpty(p.Name) &&
                        (f.Model.ToLower().Contains(p.Name.ToLower()) ||
                         p.Name.ToLower().Contains(f.Model.ToLower()) ||
                         f.Brand.ToLower().Contains(p.Name.ToLower()) ||
                         p.Name.ToLower().Contains(f.Brand.ToLower()))
                    )
                    .Sum(f =>
                    {
                        // Use the SAME calculation as InventoryLiaison
                        var allocatedCount = f.FridgeAllocation
                            .Where(a => a.ReturnDate == null || a.ReturnDate > DateOnly.FromDateTime(DateTime.Today))
                            .Sum(a => 1);

                        return f.Quantity - allocatedCount;
                    })
            }).ToList()
        }).ToList();

        var vm = new DashboardViewModel
        {
            Categories = categoriesVm
        };

        return View(vm);
    }

    // --------------------------
    // 2. Browse Category
    // --------------------------
    public async Task<IActionResult> Category(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
                .ThenInclude(p => p.Reviews)
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
            return NotFound();

        return View(category);
    }

    // --------------------------
    // 3. Product Details / Compare
    // --------------------------
    public async Task<IActionResult> ProductDetails(int id)
    {
        var product = await _context.Products
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.ProductId == id);

        if (product == null)
            return NotFound();

        return View(product);
    }

    // --------------------------
    // 4. Add to Cart
    // --------------------------
    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        var customerId = GetLoggedInCustomerId();
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (cart == null)
        {
            cart = new Cart { CustomerId = customerId, Items = new List<CartItem>() };
            _context.Carts.Add(cart);
        }

        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (cartItem != null)
            cartItem.Quantity += quantity;
        else
            cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity });

        await _context.SaveChangesAsync();

        return RedirectToAction("ViewCart");
    }

    // --------------------------
    // 5. View Cart
    // --------------------------
    public async Task<IActionResult> ViewCart()
    {
        var customerId = GetLoggedInCustomerId();
        var cart = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        return View(cart);
    }

    // --------------------------
    // 6. Checkout Process
    // --------------------------
    public async Task<IActionResult> Checkout()
    {
        var customerId = GetLoggedInCustomerId();
        var cart = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmCheckout(CheckoutViewModel model)
    {
        var customerId = GetLoggedInCustomerId();

        var order = new Order
        {
            CustomerId = customerId,
            OrderDate = DateTime.Now,
            Status = "Processing",
            DeliveryAddress = model.DeliveryAddress,
            TotalAmount = model.TotalAmount,
            Items = model.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Price = ci.Product.Price
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        // Optionally: clear cart
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        if (cart != null) _context.Carts.Remove(cart);

        await _context.SaveChangesAsync();

        return RedirectToAction("OrderConfirmation", new { id = order.OrderId });
    }

    // --------------------------
    // 7. Order Confirmation
    // --------------------------
    public async Task<IActionResult> OrderConfirmation(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        return View(order);
    }

    // GET: /Customer/AddCard
    [HttpGet]
    public IActionResult AddCard(int orderId, decimal amount)
    {
        var model = new PaymentViewModel
        {
            OrderId = orderId,
            Amount = amount,
            Method = Method.Card // Default to card method
        };

        return View(model);
    }

    // POST: /Customer/AddCard
    [HttpPost]
    public async Task<IActionResult> AddCard(PaymentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var payment = new Payment
        {
            OrderId = model.OrderId,
            Amount = model.Amount,
            Method = model.Method,
            CardNumber = model.CardNumber,
            BankReference = model.BankReference,
            PaymentDate = DateTime.Now,
            Status = "Card Added" // Change depending on your logic
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return RedirectToAction("OrderConfirmation", new { id = model.OrderId });
    }

    // --------------------------
    // 8. My Account / Orders
    // --------------------------
    public async Task<IActionResult> MyAccount()
    {
        var customerId = GetLoggedInCustomerId();
        var orders = await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .ToListAsync();

        return View(orders);
    }

    // --------------------------
    // 9. Track Order
    // --------------------------
    public async Task<IActionResult> TrackOrder(int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        return View(order);
    }

    // --------------------------
    // 10. Payment
    // --------------------------
    [HttpPost]
    public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
    {
        var payment = new Payment
        {
            OrderId = model.OrderId,
            Amount = model.Amount,
            Method = model.Method,
            PaymentDate = DateTime.Now,
            Status = "Paid" // For demo, integrate real gateway later
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return RedirectToAction("PaymentConfirmation");
    }

    public IActionResult PaymentConfirmation()
    {
        return View();
    }

    // --------------------------
    // Helper: Get Logged-in Customer Id
    // --------------------------
    private int GetLoggedInCustomerId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return 0;

        int appUserId = int.Parse(userIdClaim.Value);

        var customer = _context.Customers.FirstOrDefault(c => c.ApplicationUserId == appUserId);
        return customer?.CustomerID ?? 0;
    }

    // --------------------------
    // Upcoming Maintenance Visits
    // --------------------------
    public async Task<IActionResult> UpcomingVisits()
    {
        var customerId = GetLoggedInCustomerId();

        var visits = await _context.MaintenanceVisit
            .Include(v => v.MaintenanceRequest)
                .ThenInclude(r => r.Fridge)
            .Include(v => v.Employee)
            .Where(v => v.MaintenanceRequest.Fridge.CustomerId == customerId &&
                        (v.Status == FridgeManagementSystem.Models.TaskStatus.Scheduled || v.Status == FridgeManagementSystem.Models.TaskStatus.Rescheduled))
            .OrderBy(v => v.ScheduledDate)
            .ThenBy(v => v.ScheduledTime)
            .ToListAsync();

        return View(visits);
    }
    // --------------------------
    // View Service History for a specific fridge
    // --------------------------
    public async Task<IActionResult> FridgeServiceHistory(int fridgeId)
    {
        var customerId = GetLoggedInCustomerId();

        var visits = await _context.MaintenanceVisit
            .Include(v => v.MaintenanceRequest)
                .ThenInclude(r => r.Fridge)
                    .ThenInclude(f => f.Customer)
                        .ThenInclude(c => c.Location)
            .Include(v => v.Employee)
            .Include(v => v.MaintenanceChecklist)
            .Include(v => v.ComponentUsed)
            .Include(v => v.FaultReport)
            .Where(v => v.MaintenanceRequest.FridgeId == fridgeId &&
                        v.MaintenanceRequest.Fridge.CustomerId == customerId)
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
            .FirstOrDefault(f => f.FridgeId == fridgeId && f.CustomerId == customerId);

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

        if (!visits.Any()) return NotFound("No service history for this fridge.");

        var generator = new ServiceHistoryPdfGenerator(visits, fridge);
        var pdfBytes = generator.GeneratePdf();
        var fileName = $"ServiceHistory_{fridge.FridgeName}_{DateTime.Now:yyyyMMdd}.pdf";

        return File(pdfBytes, "application/pdf", fileName);
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
