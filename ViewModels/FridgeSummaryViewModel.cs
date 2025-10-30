using FridgeManagementSystem.Models;
namespace FridgeManagementSystem.ViewModels
{
    public class FridgeSummaryViewModel
    {
        public int TotalFridges { get; set; }
        public int Available { get; set; }
        public int Allocated { get; set; }
        public int Damaged { get; set; }
        public int Scrapped { get; set; }
    }
}
