using ProkodersECommerce.Models;
using System.Threading.Tasks;

namespace ProkodersECommerce.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(string userId);
        Task AddToCartAsync(string userId, int productId, int quantity);
        Task UpdateItemQuantityAsync(string userId, int itemId, int quantity);
        Task RemoveItemAsync(string userId, int itemId);
        Task ClearCartAsync(string userId);

    }
}
