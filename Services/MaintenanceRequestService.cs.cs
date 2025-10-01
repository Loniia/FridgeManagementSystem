using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;  
using FridgeManagementSystem.Models;

namespace FridgeManagementSystem.Services
{
    public class MaintenanceRequestService : IMaintenanceRequestService
    {
        private readonly FridgeDbContext _context;

        public MaintenanceRequestService(FridgeDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasPendingOrScheduledRequestAsync(int fridgeId)
        {
            return await _context.MaintenanceRequest.AnyAsync(r =>
                r.FridgeId == fridgeId &&
                r.IsActive &&
                (r.TaskStatus == Models.TaskStatus.Pending || r.TaskStatus == Models.TaskStatus.Scheduled));
        }

        public async Task<MaintenanceRequest?> CreateInitialRequestForAllocationAsync(int fridgeId)
        {
            if (await HasPendingOrScheduledRequestAsync(fridgeId))
                return null;

            var req = new MaintenanceRequest
            {
                FridgeId = fridgeId,
                RequestDate = DateTime.Now,
                TaskStatus = Models.TaskStatus.Pending,
                IsActive = true
            };

            _context.MaintenanceRequest.Add(req);
            await _context.SaveChangesAsync();
            return req;
        }

        public async Task<MaintenanceRequest?> CreateNextMonthlyRequestAsync(int fridgeId)
        {
            if (await HasPendingOrScheduledRequestAsync(fridgeId))
                return null;

            var req = new MaintenanceRequest
            {
                FridgeId = fridgeId,
                RequestDate = DateTime.Now.AddMonths(1),
                TaskStatus = Models.TaskStatus.Pending,
                IsActive = true
            };

            _context.MaintenanceRequest.Add(req);
            await _context.SaveChangesAsync();
            return req;
        }
    }
}

