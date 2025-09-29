namespace FridgeManagementSystem.Models
#nullable disable
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string IconClass { get; set; }

        public string RequiredRole { get; set; }

        public int Order { get; set; }

        public string MenuCategory { get; set; }
    }
}
