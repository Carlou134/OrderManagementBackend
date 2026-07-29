using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementBackend.Domain;
using OrderManagementBackend.Domain.Interfaces;
using OrderManagementBackend.Infrastructure.Data;

namespace OrderManagementBackend.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OrdersContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(OrdersContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<(IReadOnlyCollection<Product> Items, int TotalCount)> ListProducts(string? name, int page, int pageSize)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error consulting the product in database");
                throw;
            }
        }

        public async Task<bool> CreateProduct(Product product)
        {
            try
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating the product in database");
                throw;
            }
        }

        public async Task<Product?> GetProductById(int id)
        {
            try
            {
                return await _context.Product.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error loading the product from database");
                throw;
            }
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating the product");
                throw;
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var result = await GetProductById(id);

                if(result != null)
                {
                    _context.Remove(result);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating the note in the database");
                throw;
            }
        }

        public async Task<bool> IsProductInOrdersAsync(int productId)
        {
            try
            {
                return await _context.OrderProduct.AnyAsync(x => x.ProductId == productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consulting the product in database");
                throw;
            }
        }

        public async Task<IReadOnlyCollection<Product>> GetByIdsAsync(List<int> ids)
        {
            try
            {
                return await _context.Product.Where(x => ids.Contains(x.Id)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consulting the product in database");
                throw;
            }
        }
    }
}
