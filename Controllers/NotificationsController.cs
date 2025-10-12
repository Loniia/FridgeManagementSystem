using Microsoft.AspNetCore.Mvc;
using FridgeManagementSystem.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IActionResult> All(int page = 1)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var items = await _notificationService.GetAllAsync(userId, page, 50);
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return Ok();
        }
    }
}
