using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace FridgeManagementSystem.Controllers
{
    public class ReportController : Controller
    {
       
        private readonly FridgeDbContext _context;

        public ReportController(FridgeDbContext context)
        {
            _context = context;
        }

        // GET: ProcessFault
        // Add this to your ReportController
        // This will respond to the route: /Report?status=Complete
        public async Task<IActionResult> Index(string status)
        {
            // Get all reports from the database
            var allReports = _context.FaultReport.Include(f => f.Fault).AsQueryable();

            // If a status filter was provided, apply it
            if (!string.IsNullOrEmpty(status))
            {
                // Assuming your FaultReport model has a 'Status' property
                allReports = allReports.Where(r => r.StatusFilter == status);
            }

            // Pass the list of reports to the view
            return View(await allReports.ToListAsync());
        }

        // GET: ProcessFault/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultReport = await _context.FaultReport
                .Include(f => f.Fault)
                .FirstOrDefaultAsync(m => m.FaultReportId == id);
            if (faultReport == null)
            {
                return NotFound();
            }

            return View(faultReport);
        }

        // GET: ProcessFault/Create
        public IActionResult Create()
        {
            ViewData["FaultID"] = new SelectList(_context.Faults, "FaultID", "FaultDescription");
            return View();
        }

        // POST: ProcessFault/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportID,ReportDate,ReportDetails,FaultID")] FaultReport faultReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faultReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FaultID"] = new SelectList(_context.Faults, "FaultID", "FaultDescription", faultReport.FaultID);
            return View(faultReport);
        }

        // GET: ProcessFault/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultReport = await _context.FaultReport.FindAsync(id);
            if (faultReport == null)
            {
                return NotFound();
            }
            ViewData["FaultID"] = new SelectList(_context.Faults, "FaultID", "FaultDescription", faultReport.FaultID);
            return View(faultReport);
        }

        // POST: ProcessFault/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportID,ReportDate,ReportDetails,FaultID")] FaultReport faultReport)
        {
            if (id != faultReport.FaultReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faultReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaultReportExists(faultReport.FaultReportId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FaultID"] = new SelectList(_context.Faults, "FaultID", "FaultDescription", faultReport.FaultID);
            return View(faultReport);
        }

        // GET: ProcessFault/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultReport = await _context.FaultReport
                .Include(f => f.Fault)
                .FirstOrDefaultAsync(m => m.FaultReportId == id);
            if (faultReport == null)
            {
                return NotFound();
            }

            return View(faultReport);
        }

        // POST: ProcessFault/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faultReport = await _context.FaultReport.FindAsync(id);
            if (faultReport != null)
            {
                _context.FaultReport.Remove(faultReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FaultReportExists(int id)
        {
            return _context.FaultReport.Any(e => e.FaultReportId == id);
        }
    }
}
