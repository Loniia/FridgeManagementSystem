using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;   // <-- replace with your namespace
using FridgeManagementSystem.Models; // <-- replace with your namespace
using System.Linq;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Areas.Administration.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = Roles.Admin)] // Only Admins can manage locations
    public class ManageLocationController : Controller
    {
        private readonly FridgeDbContext _context;

        public ManageLocationController(FridgeDbContext context)
        {
            _context = context;
        }

        // 1️⃣ LIST all active locations
        public async Task<IActionResult> Index()
        {
            var locations = await _context.Locations
                .Where(l => l.IsActive) // Only show active ones
                .Include(l => l.Customer)
                .Include(l => l.Employee)
                .Include(l => l.Fridge)
                .ToListAsync();

            return View(locations);
        }

        // 2️⃣ DETAILS for one location
        public async Task<IActionResult> Details(int id)
        {
            var location = await _context.Locations
                .Include(l => l.Customer)
                .Include(l => l.Employee)
                .Include(l => l.Fridge)
                .FirstOrDefaultAsync(m => m.LocationId == id && m.IsActive);

            if (location == null) return NotFound();

            return View(location);
        }

        // 3️⃣ CREATE a new location
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddLocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var location = new Location
                {
                    Address = model.Address,
                    City = model.City,
                    Province = model.Province,
                    PostalCode = model.PostalCode,
                    IsActive = true
                };
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index)); 
        }

        // 4️⃣ EDIT an existing location
        public async Task<IActionResult> Edit(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null || !location.IsActive) return NotFound();
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Location location)
        {
            if (id != location.LocationId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Make sure we don’t accidentally overwrite IsActive = false
                    var existing = await _context.Locations.AsNoTracking()
                        .FirstOrDefaultAsync(l => l.LocationId == id);

                    if (existing == null) return NotFound();

                    location.IsActive = existing.IsActive; // preserve soft delete status
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Locations.Any(e => e.LocationId == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // 5️⃣ SOFT DELETE (mark inactive instead of removing)
        public async Task<IActionResult> Delete(int id)
        {
            var location = await _context.Locations
                .FirstOrDefaultAsync(l => l.LocationId == id && l.IsActive);

            if (location == null) return NotFound();

            return View(location);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null) return NotFound();

            // Soft delete instead of physical delete
            location.IsActive = false;
            _context.Update(location);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // 6️⃣ REACTIVATE a soft-deleted location
        public async Task<IActionResult> Reactivate(int id)
        {
            var location = await _context.Locations
                .FirstOrDefaultAsync(l => l.LocationId == id && !l.IsActive);

            if (location == null) return NotFound();

            location.IsActive = true;
            _context.Update(location);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // LIST inactive locations
        public async Task<IActionResult> Inactive()
        {
            var locations = await _context.Locations
                .Where(l => !l.IsActive)
                .ToListAsync();

            return View(locations);
        }

    }
}
