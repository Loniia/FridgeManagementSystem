using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class ManageSupplierController : Controller
    {
        private readonly FridgeDbContext _context;

        public ManageSupplierController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: Administrator/ManageSupplier
        public async Task<IActionResult> Index()
        {
            var suppliers = await _context.Suppliers
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
            return View(suppliers);
        }

        // GET: Administrator/ManageSupplier/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id && m.IsActive);

            if (supplier == null) return NotFound();
            return View(supplier);
        }

        // GET: Administrator/ManageSupplier/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrator/ManageSupplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.IsActive = true;
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // Other actions (Edit, Delete) would go here...
    }
}
