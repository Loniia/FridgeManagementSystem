using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class FinancialAccount
    {
        [Key]
        public int FinancialAccountId { get; set; }

        // Holds money in the smallest meaningful unit (decimal ok for demo)
        public decimal Balance { get; set; } = 0m;

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // optional: friendly name (e.g., "Main System Account")
        [StringLength(100)]
        public string Name { get; set; } = "Main Financial Account";
    }
}
