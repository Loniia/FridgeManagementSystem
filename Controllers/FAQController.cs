using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
#nullable disable

namespace FridgeManagementSystem.Controllers
{
    public class FAQController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Search(string searchTerm)
        {
            return Json(new { message = "Search handled client-side" });
        }
    }

    public class FAQSection
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<FAQItem> Questions { get; set; }
    }

    public class FAQItem
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}