namespace FridgeManagementSystem.Models
#nullable disable
{
    public class ManageFridgeViewModel
    {
        public IEnumerable<Fridge> AllFridges { get; set; }
        public IEnumerable<Fridge> ScrappedFridges { get; set; }
        public IEnumerable<Fridge> ReturnedFridges { get; set; }
    }
}
