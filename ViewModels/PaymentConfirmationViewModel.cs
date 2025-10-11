using FridgeManagementSystem.Models;
using FridgeManagementSystem.Data;
#nullable disable
namespace FridgeManagementSystem.ViewModels
{
    public class PaymentConfirmationViewModel
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
