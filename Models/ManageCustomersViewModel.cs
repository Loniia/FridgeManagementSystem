using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data;
#nullable disable
namespace FridgeManagementSystem.Models
{
    public class ManageCustomersViewModel
    { 
        public List<Customer> AllCustomers { get; set; }
        public Customer SelectedCustomer { get; set; }
    }
}
