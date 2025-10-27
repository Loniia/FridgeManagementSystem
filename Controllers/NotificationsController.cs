using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FridgeManagementSystem.Controllers
{
    public class NotificationsController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService, UserManager<ApplicationUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }
        private async Task<int> GetAppUserIdAsync()
        {
            var appUser = await _userManager.GetUserAsync(User);
            return appUser?.Id ?? 0;
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var userId = await GetAppUserIdAsync();
            if (userId == 0) return RedirectToAction("Login", "Account");

            var notifications = await _notificationService.GetAllAsync(userId);
            return View("All", notifications);
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
            var userId = await GetAppUserIdAsync();
            if (userId == 0) return Json(new { });

            var notifications = await _notificationService.GetUnreadAsync(userId, 5);
            return Json(notifications.Select(n => new
            {
                n.NotificationId,
                n.Message,
                n.Url
            }));
        }

        [HttpGet]
        public async Task<IActionResult> UnreadCount()
        {
            var userId = await GetAppUserIdAsync();
            if (userId == 0) return Json(0);

            var count = (await _notificationService.GetUnreadAsync(userId)).Count();
            return Json(count);
        }

    }
}
