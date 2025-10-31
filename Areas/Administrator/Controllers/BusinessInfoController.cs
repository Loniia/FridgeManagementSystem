using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data; // adjust namespace
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // adjust namespace

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Admin")]
    public class BusinessInfoController : Controller
    {
        private readonly FridgeDbContext _context;
        private readonly ILogger<BusinessInfoController> _logger;

        public BusinessInfoController(FridgeDbContext context, ILogger<BusinessInfoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: BusinessInfo
        public async Task<IActionResult> Index()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (businessInfo == null)
                {
                    _logger.LogInformation("No business info found.");
                    TempData["ErrorMessage"] = "No business information available.";
                    return View();
                }

                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving business information");
                TempData["ErrorMessage"] = "An error occurred while retrieving business information.";
                return View();
            }
        }

        // GET: BusinessInfo/About - Public facing "About Us" page
        [AllowAnonymous]
        public async Task<IActionResult> About()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (businessInfo == null)
                {
                    return View("NoBusinessInfo");
                }

                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading About page");
                return View("Error");
            }
        }

        // GET: BusinessInfo/Services - Public facing "What We Do" page
        [AllowAnonymous]
        public async Task<IActionResult> Services()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (businessInfo == null)
                {
                    return View("NoBusinessInfo");
                }

                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Services page");
                return View("Error");
            }
        }
    }
}
