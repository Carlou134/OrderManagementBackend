using Microsoft.EntityFrameworkCore;
using OrderManagementBackend.Domain;
using OrderManagementBackend.Domain.Interfaces;
using OrderManagementBackend.Infrastructure.Data;

namespace OrderManagementBackend.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OrdersContext _context;

        public ProductRepository(OrdersContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyCollection<Product> Items, int TotalCount)> ListProducts(string? name, int page, int pageSize)
        {
            var query = _context.Product.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> CreateProduct(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Product.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var result = await GetProductById(id);

            if (result != null)
            {
                _context.Remove(result);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> IsProductInOrdersAsync(int productId)
        {
            return await _context.OrderProduct.AnyAsync(x => x.ProductId == productId);
        }

        public async Task<IReadOnlyCollection<Product>> GetByIdsAsync(List<int> ids)
        {
            return await _context.Product.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
    }
}
