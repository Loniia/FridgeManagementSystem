using Microsoft.AspNetCore.Mvc;
using PurchasingSubsystem.Data;
using PurchasingSubsystem.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace PurchasingSubsystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
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
   