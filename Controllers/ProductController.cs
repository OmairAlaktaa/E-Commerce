using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProkodersECommerce.Interfaces;
using ProkodersECommerce.Models;

namespace ProkodersECommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;
        private const int PageSize = 10;

        public ProductController(IProductRepository productRepository, IMemoryCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<IActionResult> Index(string searchTerm, string sortBy, int page = 1)
        {
            var cacheKey = "productList";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> allProducts))
            {
                allProducts = await _productRepository.GetAllAsync();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _cache.Set(cacheKey, allProducts, cacheOptions);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                allProducts = allProducts.Where(p =>
                    p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            allProducts = sortBy switch
            {
                "name_desc" => allProducts.OrderByDescending(p => p.Name),
                "price_asc" => allProducts.OrderBy(p => p.Price),
                "price_desc" => allProducts.OrderByDescending(p => p.Price),
                _ => allProducts.OrderBy(p => p.Name)
            };

            var totalItems = allProducts.Count();
            var products = allProducts
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortBy = sortBy;

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
