using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProkodersECommerce.Data;
using ProkodersECommerce.Enums;
using ProkodersECommerce.Interfaces;
using ProkodersECommerce.Models;
using ProkodersECommerce.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProkodersECommerce.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _context;

        public CheckoutController(ICartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartAsync(userId);

            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var viewModel = new OrderViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(OrderViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartAsync(userId);

            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            foreach (var item in cart.Items)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                {
                    TempData["Error"] = $"Insufficient stock for {product?.Name ?? "a product"}.";
                    return RedirectToAction("Index", "Cart");
                }
            }

            var order = new Order
            {
                UserId = userId,
                FullName = model.FullName,
                Address = model.Address,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTime.Now,
                TotalAmount = cart.Items.Sum(i => i.Product.Price * i.Quantity),
                Status = OrderStatus.Processing,
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            };

            foreach (var item in cart.Items)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);
                product.Stock -= item.Quantity;
                _context.Products.Update(product);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            await _cartService.ClearCartAsync(userId);

            return RedirectToAction("Confirmation", new { id = order.Id });
        }


        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
