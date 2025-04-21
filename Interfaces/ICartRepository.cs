using ProkodersECommerce.Models;

namespace ProkodersECommerce.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task<Cart> CreateCartAsync(string userId);
        Task AddItemAsync(CartItem item);
        Task RemoveItemAsync(int cartItemId);
        Task SaveAsync();
    }
}
