using System.ComponentModel.DataAnnotations;
#nullable disable

namespace FridgeManagementSystem.Models
{
    public class Location
    {
        [Key] 
        public int LoationId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }    
        public string Province { get; set; }
        public string PostalCode { get; set; }

    }
}
