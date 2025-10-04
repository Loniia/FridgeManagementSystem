using FridgeManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class AddLocationViewModel
    {
        [Required] public string Address { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Province { get; set; }
        [Required] public string PostalCode { get; set; }
    }
}
