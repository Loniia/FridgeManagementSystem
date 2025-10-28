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
        public int TotalCustomers { get; set; }
        public int TotalFridges { get; set; }
        public int TotalAllocatedFridges { get; set; }
        public int TotalScrappedFridges { get; set; }
        public int TotalSuppliers { get; set; }
       
    }
}
