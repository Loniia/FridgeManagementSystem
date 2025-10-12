using System.Collections.Generic;
using System.Threading.Tasks;
using FridgeManagementSystem.Models;

namespace FridgeManagementSystem.Services
{
    public interface INotificationService
    {
        Task CreateAsync(int userId, string message, string url = null);
        Task<IEnumerable<Notification>> GetUnreadAsync(int userId, int take = 20);
        Task<IEnumerable<Notification>> GetAllAsync(int userId, int page = 1, int pageSize = 50);
        Task MarkAsReadAsync(int notificationId);
    }
}
