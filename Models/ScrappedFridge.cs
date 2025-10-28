using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class ScrappedFridge
    {
        [Key, ForeignKey("Fridge")]
        public int FridgeID { get; set; }
        [Required, StringLength(200)]
        public string Reason { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly? ScrapDate { get; set; }

        //Navigation Property
        public virtual Fridge Fridge { get; set; }
    }
}
