using FridgeManagementSystem.Models;
namespace FridgeManagementSystem.ViewModels
{
    public class InventoryDashboardViewModel
    {
        public List<FridgeViewModel> ActiveFridges { get; set; } = new();
        public List<FridgeViewModel> ReturnedFridges { get; set; } = new();
        public List<FridgeViewModel> ScrappedFridges { get; set; } = new();
    }
}
