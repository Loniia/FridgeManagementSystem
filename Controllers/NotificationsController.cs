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
            return RedirectToAction("All");
        }
        [HttpGet]
        public async Task<IActionResult> Unread()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var notifications = await _notificationService.GetUnreadAsync(userId, 5);
            return Json(notifications.Select(n => new { n.NotificationId, n.Message, n.Url }));
        }

        [HttpGet]
        public async Task<IActionResult> UnreadCount()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var count = (await _notificationService.GetUnreadAsync(userId)).Count();
            return Json(count);
        }

    }
}
