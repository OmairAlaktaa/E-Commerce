using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProkodersECommerce.Data;
using ProkodersECommerce.Interfaces;
using ProkodersECommerce.Models;

namespace ProkodersECommerce.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public ProductRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _context.Products.ToListAsync();

        public async Task<Product> GetByIdAsync(int id) =>
            await _context.Products.FindAsync(id);

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await SaveAsync();
            _cache.Remove("productList");
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _cache.Remove("productList");
        }

        public void Remove(Product product)
        {
            _context.Products.Remove(product);
            _cache.Remove("productList");
        }

        public async Task SaveAsync() =>
            await _context.SaveChangesAsync();
    }
}
