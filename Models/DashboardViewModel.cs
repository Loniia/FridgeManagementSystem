using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.ViewModels;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class DashboardViewModel
    {
        //How it works:The controller gathers data(counts, totals, lists, etc.) from the database.
        //It fills a DashboardViewModel object with these values.
        //The dashboard view reads this ViewModel and displays the data neatly in cards, tables, or charts.

        public int TotalCustomers { get; set; }
        public int TotalFridges { get; set; }
        public int TotalAllocatedFridges { get; set; }
        public int TotalScrappedFridges { get; set; }
        public int TotalSuppliers { get; set; }
        
      // need to add recommended fridges
    }
}
