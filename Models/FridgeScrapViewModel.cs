using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeManagementSystem.Models
#nullable disable
{
    public class FridgeScrapViewModel
    {
        [ForeignKey("Fridge")]
        public int FridgeId { get; set; }
        public string Model { get; set; }
        public string Status { get; set; }
    }
}
