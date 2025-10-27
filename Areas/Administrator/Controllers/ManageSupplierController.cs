using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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


        // GET: ManageSupplier
        public async Task<IActionResult> Index(string search)
        {
            var suppliers = _context.Suppliers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                suppliers = suppliers.Where(s =>
                    s.Name.Contains(search) ||
                    s.Email.Contains(search) ||
                    s.ContactPerson.Contains(search));
            }

            var model = new
            {
                Total = await _context.Suppliers.CountAsync(),
                Active = await _context.Suppliers.CountAsync(s => s.IsActive),
                Inactive = await _context.Suppliers.CountAsync(s => !s.IsActive),
                Suppliers = await suppliers.OrderByDescending(s => s.SupplierID).ToListAsync()
            };

            return View(model);
        }

        // GET: ManageSupplier/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.Quotations)
                .Include(s => s.PurchaseOrders)
                .FirstOrDefaultAsync(s => s.SupplierID == id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // POST: ManageSupplier/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return NotFound();

            supplier.IsActive = !supplier.IsActive;
            _context.Update(supplier);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

