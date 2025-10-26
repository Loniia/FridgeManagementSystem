using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using FridgeManagementSystem.Data;

namespace FridgeManagementSystem.Helpers
{
    public static class CartHelper
    {
        public static async Task ClearCartAsync(FridgeDbContext context, int customerId)
        {
            var cart = await context.Carts
                .Include(c => c.CartItems)
                .Where(c => c.CustomerID == customerId)
                .OrderByDescending(c => c.CartId) // ✅ Always get latest cart
                .FirstOrDefaultAsync();

            if (cart != null)
            {
                foreach (var item in cart.CartItems)
                {
                    item.IsDeleted = true;
                }

                cart.IsActive = false; // ✅ Ensure outbound update

                await context.SaveChangesAsync();
            }
        }
    }
}
