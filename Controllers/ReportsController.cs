using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FridgeManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly FridgeDbContext _context;

        public ReportsController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: /Reports
        // Main dashboard view action (renders Views/Reports/Index.cshtml)
        public async Task<IActionResult> Index()
        {
            // --- SUMMARY NUMBERS FOR TOP CARDS ---

            var totalCustomers = await _context.Customers.CountAsync();
            var totalFridges = await _context.Fridge.CountAsync();
            var activeFaults = await _context.Faults
                .Where(f => f.Status != null && f.Status.ToLower() != "resolved")
                .CountAsync();
            var totalSales = await _context.Orders
                .Select(o => (decimal?)o.TotalAmount)
                .SumAsync() ?? 0m;
            var totalPurchases = await _context.PurchaseOrders
                .Select(po => (decimal?)po.TotalAmount)
                .SumAsync() ?? 0m;
            var netProfit = totalSales - totalPurchases;

            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalFridges = totalFridges;
            ViewBag.ActiveFaults = activeFaults;
            ViewBag.TotalSales = totalSales;
            ViewBag.TotalPurchases = totalPurchases;
            ViewBag.NetProfit = netProfit;

            return View();
        }

        // -----------------------
        // JSON endpoints for JS
        // -----------------------

        [HttpGet]
        public async Task<JsonResult> TopCustomers(int top = 10)
        {
            var q = await _context.Orders
                .Where(o => o.CustomerID != 0)
                .GroupBy(o => new { o.CustomerID })
                .Select(g => new
                {
                    CustomerId = g.Key.CustomerID,
                    TotalAmount = g.Sum(x => x.TotalAmount),
                    TotalOrders = g.Count()
                })
                .OrderByDescending(x => x.TotalAmount)
                .Take(top)
                .ToListAsync();

            var customerIds = q.Select(x => x.CustomerId).ToList();

            var customers = await _context.Customers
                .Where(c => customerIds.Contains(c.CustomerID))
                .Select(c => new { c.CustomerID, c.FullName })
                .ToListAsync();

            var result = q.Select(item =>
            {
                var cust = customers.FirstOrDefault(c => c.CustomerID == item.CustomerId);
                return new
                {
                    item.CustomerId,
                    CustomerName = cust != null ? cust.FullName : "Unknown",
                    item.TotalOrders,
                    item.TotalAmount
                };
            });

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> MonthlySales(int months = 12)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months + 1);
            var data = await _context.Orders
                .Where(o => o.OrderDate >= startDate)
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalAmount = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> MonthlyPurchases(int months = 12)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months + 1);
            var data = await _context.PurchaseOrders
                .Where(po => po.OrderDate >= startDate)
                .GroupBy(po => new { po.OrderDate.Year, po.OrderDate.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalAmount = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> TopFaults(int top = 5)
        {
            var data = await _context.Faults
                .GroupBy(f => f.FaultDescription)
                .Select(g => new
                {
                    FaultType = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(top)
                .ToListAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> InventoryByModel()
        {
            var data = await _context.Fridge
                .GroupBy(f => f.Model)
                .Select(g => new
                {
                    Model = g.Key,
                    Count = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> TopBrands(int top = 5)
        {
            var data = await _context.Fridge
                .GroupBy(f => f.Brand)
                .Select(g => new
                {
                    Brand = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(top)
                .ToListAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> LocationMapData()
        {
            var data = await _context.Locations
                .Where(l => l.IsActive)
                .Include(l => l.Fridge)
                .Include(l => l.Customer)
                .Include(l => l.Employee)
                .Select(l => new
                {
                    l.Province,
                    l.City,
                    l.Address,
                    l.PostalCode,
                    l.Latitude,
                    l.Longitude,
                    FridgeCount = l.Fridge.Count(f => f.IsActive),
                    CustomerCount = l.Customer.Count(c => c.IsActive),
                    EmployeeCount = l.Employee.Count(e => e.Status=="Active")
                })
                .ToListAsync();

            return Json(data);
        }

    }
}
