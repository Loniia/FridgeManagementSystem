using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;

namespace FridgeManagementSystem.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly FridgeDbContext _context;

        public PurchaseController(FridgeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get counts for dashboard
            ViewBag.PurchaseRequestsCount = await _context.PurchaseRequests.CountAsync();
            ViewBag.SuppliersCount = await _context.Suppliers.CountAsync();
            ViewBag.QuotationsCount = await _context.Quotations.CountAsync();
            ViewBag.PurchaseOrdersCount = await _context.PurchaseOrders.CountAsync();

            return View();
        }
    }
}
   