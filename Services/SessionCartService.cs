using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProkodersECommerce.Interfaces;
using ProkodersECommerce.Models;

namespace ProkodersECommerce.Services
{
    public class SessionCartService : ISessionCartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionKey = "CartItems";

        public SessionCartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<SessionCartItem>> GetCartAsync()
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            var json = session.GetString(SessionKey);
            return string.IsNullOrEmpty(json)
                ? new List<SessionCartItem>()
                : JsonConvert.DeserializeObject<List<SessionCartItem>>(json)!;
        }

        public async Task AddToCartAsync(int productId, int quantity)
        {
            var cart = await GetCartAsync();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
                item.Quantity += quantity;
            else
                cart.Add(new SessionCartItem { ProductId = productId, Quantity = quantity });

            SaveCart(cart);
        }

        public async Task UpdateItemQuantityAsync(int productId, int quantity)
        {
            var cart = await GetCartAsync();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
                item.Quantity = quantity;

            SaveCart(cart);
        }

        public async Task RemoveItemAsync(int productId)
        {
            var cart = await GetCartAsync();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
                cart.Remove(item);

            SaveCart(cart);
        }

        public async Task ClearCartAsync()
        {
            SaveCart(new List<SessionCartItem>());
        }

        private void SaveCart(List<SessionCartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            session.SetString(SessionKey, JsonConvert.SerializeObject(cart));
        }
    }
}
