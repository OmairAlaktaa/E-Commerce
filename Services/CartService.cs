using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using ProkodersECommerce.Data;
using ProkodersECommerce.Hubs;
using ProkodersECommerce.Interfaces;
using ProkodersECommerce.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProkodersECommerce.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<CartHub> _hubContext;

        public CartService(ApplicationDbContext context, IHubContext<CartHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        private async Task NotifyCartChanged(string userId)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveCartUpdate");
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartAsync(userId);

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _context.Update(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                };
                cart.Items.Add(newItem);
            }

            await _context.SaveChangesAsync();
            await NotifyCartChanged(userId);
        }

        public async Task UpdateItemQuantityAsync(string userId, int itemId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                item.Quantity = quantity;
                _context.Update(item);
                await _context.SaveChangesAsync();
                await NotifyCartChanged(userId);
            }
        }

        public async Task RemoveItemAsync(string userId, int itemId)
        {
            var cart = await GetCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
                await NotifyCartChanged(userId);
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetCartAsync(userId);
            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
            await NotifyCartChanged(userId);
        }
    }
}
