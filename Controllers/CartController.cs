using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProkodersECommerce.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProkodersECommerce.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartAsync(userId);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.AddToCartAsync(userId, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Dictionary<int, int> quantities, int? removeItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (removeItemId.HasValue)
            {
                await _cartService.RemoveItemAsync(userId, removeItemId.Value);
                return RedirectToAction("Index");
            }

            foreach (var kvp in quantities)
            {
                int itemId = kvp.Key;
                int quantity = kvp.Value;

                if (quantity < 1)
                    continue;

                await _cartService.UpdateItemQuantityAsync(userId, itemId, quantity);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.RemoveItemAsync(userId, itemId);
            return RedirectToAction("Index");
        }
    }
}
