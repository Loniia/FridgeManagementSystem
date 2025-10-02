namespace FridgeManagementSystem.Models
#nullable disable
{
    public class FridgeReceiveViewModel
    {
        public string Model { get; set; }
        public string Brand { get; set; }
        public int SelectedSupplierID { get; set; }
        public List<Supplier> Suppliers { get; set; }
    }
}
