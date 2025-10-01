using FridgeManagementSystem.Models;

namespace FridgeManagementSystem.Services
{
    public interface IMaintenanceRequestService
    {
        Task<bool> HasPendingOrScheduledRequestAsync(int fridgeId);
        Task<MaintenanceRequest?> CreateInitialRequestForAllocationAsync(int fridgeId);
        Task<MaintenanceRequest?> CreateNextMonthlyRequestAsync(int fridgeId);
    }
}
