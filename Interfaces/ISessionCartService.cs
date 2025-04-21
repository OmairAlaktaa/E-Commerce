using ProkodersECommerce.Models;

namespace ProkodersECommerce.Interfaces
{
    public interface ISessionCartService
    {
        Task<List<SessionCartItem>> GetCartAsync();
        Task AddToCartAsync(int productId, int quantity);
        Task UpdateItemQuantityAsync(int productId, int quantity);
        Task RemoveItemAsync(int productId);
        Task ClearCartAsync();
    }
}
