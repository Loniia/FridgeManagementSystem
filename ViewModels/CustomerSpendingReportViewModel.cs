using System.Collections.Generic;
using FridgeManagementSystem.Models;

namespace FridgeManagementSystem.ViewModels
{
    public class CustomerSpendingReportViewModel
    {
        public string CustomerName { get; set; }
        public decimal TotalSpent { get; set; }
        public int TotalFridges { get; set; }
        public List<Order> Orders { get; set; }
    }
}
