using System;
using System.Collections.Generic;
using FridgeManagementSystem.Models;

namespace FridgeManagementSystem.ViewModels
{
    public class AdminCustomerSpendingReportViewModel
    {
        public List<CustomerSpendingReportViewModel> Customers { get; set; } = new();
    }
}
