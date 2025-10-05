using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FridgeManagementSystem.Controllers
{
    [Area("FaultTechSubsystem")]
    public class FaultTechController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FridgeDbContext _context;

        public FaultTechController(ILogger<HomeController> logger, FridgeDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        // GET: Faults
        [HttpGet]
        public IActionResult Index()
        {
            return View(); // This should return Views/Home/Index.cshtml
        }
        // GET: Home/About - About us page
        public IActionResult About()
        {
            return View();
        }

        // GET: Home/Services - Services page
        public IActionResult Services()
        {
            return View();
        }

        // GET: Home/Contact - Contact us page
        public IActionResult Contact()
        {
            return View();
        }

        //// GET: Home/Login - Login page (if you want it on home)
        //public IActionResult Login()
        //{
        //    return View();
        //}

        // GET: Home/Dashboard - Redirect to proper dashboard
        public IActionResult Dashboard()
        {
            // Redirect to the actual dashboard in FaultsController
            return RedirectToAction("Dashboard", "Faults");
        }

        // Privacy page
        public IActionResult Privacy()
        {
            return View();
        }

        // Error handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}