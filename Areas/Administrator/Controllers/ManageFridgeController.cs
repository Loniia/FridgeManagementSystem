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
            var fridgesInStock = await _context.Fridge
                .Where(f => f.IsActive) // optional filter
                .ToListAsync();

            return View(fridgesInStock);
        }

        // ✅ Request a fridge (updates status in DB)
        public IActionResult RequestFridge()
        {
            //var fridge = await _context.Fridge.FindAsync(fridgeId);
            //if (fridge == null) return NotFound();

            //fridge.Status = "Requested";
            //await _context.SaveChangesAsync();

            //TempData["Success"] = $"Fridge '{fridge.FridgeType ?? fridge.Model}' requested successfully!";
            //return RedirectToAction(nameof(Index));
            return View();
        }
        [HttpPost]
        public IActionResult SubmitFridgeRequest(string Model, string Reason)
        {
            // Save request to DB or handle logic
            TempData["Success"] = $"Your request for '{Model}' has been submitted successfully!";
            return RedirectToAction("Index");
        }

    }
}



//using FridgeManagementSystem.Data;
//using FridgeManagementSystem.Models;
//using FridgeManagementSystem.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;

//namespace FridgeManagementSystem.Areas.Administrator.Controllers
//{
//    [Area("Administrator")]
//    [Authorize(Roles = Roles.Admin)]
//    public class ManageFridgeController : Controller
//    {
//        private readonly ICustomerManagementSubSystem _customerSubSystem;

//        // ✅ Constructor name must match the class
//        public ManageFridgeController(ICustomerManagementSubSystem customerSubSystem)
//        {
//            _customerSubSystem = customerSubSystem;
//        }

//        // GET: ManageFridge/Index
//        public async Task<IActionResult> Index()
//        {
//            // Get all fridges in stock from the subsystem
//            var fridgesInStock = await _customerSubSystem.GetAllFridgesAsync();
//            return View(fridgesInStock);
//        }

//        // GET: ManageFridge/RequestFridge/5
//        public async Task<IActionResult> RequestFridge(int fridgeId)
//        {
//            var fridge = await _customerSubSystem.GetFridgeByIdAsync(fridgeId);
//            if (fridge == null) return NotFound();

//            // Call your subsystem method to request the fridge
//            await _customerSubSystem.RequestFridgeAsync(fridgeId);

//            TempData["Success"] = $"Fridge {fridge.FridgeType} requested successfully!";
//            return RedirectToAction(nameof(Index));
//        }