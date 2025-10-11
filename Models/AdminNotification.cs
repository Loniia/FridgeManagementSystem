namespace FridgeManagementSystem.Models
{
    public class AdminNotification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
