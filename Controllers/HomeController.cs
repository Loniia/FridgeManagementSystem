using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FridgeDbContext _context;

        public HomeController(ILogger<HomeController> logger, FridgeDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos.AsNoTracking().FirstOrDefaultAsync();
                ViewBag.BusinessInfo = businessInfo;
                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading home page");
                return View();
            }
        }

        public async Task<IActionResult> Privacy()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos.AsNoTracking().FirstOrDefaultAsync();
                ViewBag.BusinessInfo = businessInfo;
                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading privacy page");
                return View();
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> About()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos.AsNoTracking().FirstOrDefaultAsync();
                if (businessInfo == null) return View("NoBusinessInfo");
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
                var businessInfo = await _context.BusinessInfos.AsNoTracking().FirstOrDefaultAsync();
                if (businessInfo == null) return View("NoBusinessInfo");
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
                var businessInfo = await _context.BusinessInfos.AsNoTracking().FirstOrDefaultAsync();
                if (businessInfo == null) return View("NoBusinessInfo");
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
                var businessInfo = await _context.BusinessInfos.AsNoTracking().FirstOrDefaultAsync();
                if (businessInfo == null) return View("NoBusinessInfo");
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

        // -----------------------
        // FAQ Section
        // -----------------------
        [AllowAnonymous]
        public IActionResult FAQ()
        {
            var faqs = new List<(string Question, string Answer)>
            {
                ("How do I create an account?", "Click the Sign Up button and follow the instructions."),
                ("How can I track my fridge order?", "Go to Orders > Track Order and enter your order ID."),
                ("What is the warranty period?", "All fridges have a 2-year warranty from the date of purchase."),
                ("Can I request installation?", "Yes! Choose installation during checkout or contact support.")
            };

            return View(faqs);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult FAQMessage(string email, string message)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message))
            {
                TempData["MessageError"] = "Please fill out both email and message.";
                return RedirectToAction("FAQ");
            }

            // TODO: Save message to DB or send email
            TempData["MessageSuccess"] = "Thank you! Your message has been received.";
            return RedirectToAction("FAQ");
        }

        // -----------------------
        // DEMO AI Chat Response
        // -----------------------
        [HttpPost]
        [AllowAnonymous]
        public IActionResult AIResponse(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return Json(new { success = false, response = "Please enter a question." });

            // Simple demo reply (no Azure or OpenAI needed)
            string demoReply = $"You asked: '{userMessage}'. This is a demo AI response.";

            return Json(new { success = true, response = demoReply });
        }
    }
}
