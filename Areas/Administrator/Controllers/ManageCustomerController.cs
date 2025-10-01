using Microsoft.AspNetCore.Mvc;

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    public class ManageCustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
