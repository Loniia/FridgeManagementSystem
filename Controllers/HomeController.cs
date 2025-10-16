using System.Diagnostics;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;

namespace FridgeManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FridgeDbContext _context;

        public HomeController(ILogger<HomeController> logger, FridgeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                ViewBag.BusinessInfo = businessInfo;
                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading business info for home page");
                return View();
            }
        }

        public async Task<IActionResult> Privacy()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                ViewBag.BusinessInfo = businessInfo;
                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading business info for privacy page");
                return View();
            }
        }

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

                ViewBag.BusinessInfo = businessInfo;
                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading About page");
                return View("Error");
            }
        }

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

                ViewBag.BusinessInfo = businessInfo;
                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Services page");
                return View("Error");
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Contact()
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

                ViewBag.BusinessInfo = businessInfo;
                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Contact page");
                return View("Error");
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Help()
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

                ViewBag.BusinessInfo = businessInfo;
                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Help page");
                return View("Error");
            }
        }

        public IActionResult Team() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}