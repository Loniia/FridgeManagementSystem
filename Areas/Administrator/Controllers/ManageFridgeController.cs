using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = Roles.Admin)]
    public class ManageFridgeController : Controller
    {
        private readonly ICustomerManagementSubSystem _customerSubSystem;
        private readonly FridgeDbContext _context;

        public ManageFridgeController(ICustomerManagementSubSystem customerSubSystem, FridgeDbContext context)
        {
            _customerSubSystem = customerSubSystem;
            _context = context;
        }

        // ✅ Load all fridges directly from the database (not hardcoded)
        public async Task<IActionResult> Index()
        {
            var allFridges = await _context.Fridge
                .Where(f => f.IsActive)
                .ToListAsync();

            var scrappedFridges = await _context.Fridge
                .Where(f => f.Status == "Scrapped")
                .ToListAsync();

            var returnedFridges = await _context.Fridge
                .Where(f => f.Status == "Returned")
                .ToListAsync();

            var viewModel = new ManageFridgeViewModel
            {
                AllFridges = allFridges,
                ScrappedFridges = scrappedFridges,
                ReturnedFridges = returnedFridges
            };

            return View(viewModel);
        }


    }
}