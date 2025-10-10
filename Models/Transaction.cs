using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [ForeignKey(nameof(Payment))]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Success";
    }
}
