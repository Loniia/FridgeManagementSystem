namespace FridgeManagementSystem.Models
{
    public class PaymentViewModel
    {
        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public Method Method { get; set; }
    }
}
