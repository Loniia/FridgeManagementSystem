using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FridgeManagementSystem.Services
{
    public class NotificationService : INotificationService
    {
        private readonly FridgeDbContext _db;
        public NotificationService(FridgeDbContext db) { _db = db; }

        public async Task CreateAsync(int userId, string message, string url = null)
        {
            var n = new Notification
            {
                UserId = userId,
                Message = message,
                Url = url,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            _db.Notifications.Add(n);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadAsync(int userId, int take = 20)
        {
            return await _db.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllAsync(int userId, int page = 1, int pageSize = 50)
        {
            return await _db.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var n = await _db.Notifications.FindAsync(notificationId);
            if (n == null) return;
            n.IsRead = true;
            await _db.SaveChangesAsync();
        }
    }
}
