using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementBackend.Domain;
using OrderManagementBackend.Domain.Interfaces;
using OrderManagementBackend.Infrastructure.Data;

namespace OrderManagementBackend.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public OrderRepository(OrdersContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<Order>> ListOrders()
        {
            try
            {
                return await _context.Order.Include(x => x.OrderProducts).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consulting the order in database");
                throw;
            }
        }

        public async Task<bool> CreateOrder(Order order)
        {
            try
            {
                _context.Order.Add(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating the order in database");
                throw;
            }
        }

        public async Task<Order?> GetOrderById(int id)
        {
            try
            {
                return await _context.Order.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading the order from database");
                throw;
            }
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating the order");
                throw;
            }
        }

        public async Task<bool> DeleteOrder(int id)
        {
            try
            {
                var order = await GetOrderById(id);

                if(order != null)
                {
                    _context.Remove(order);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating the order in the database");
                throw;
            }
        }

        public async Task<bool> IsOrderInOrdersAsync(int orderId)
        {
            try
            {
                return await _context.OrderProduct.AnyAsync(x => x.OrderId == orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consulting the order in database");
                throw;
            }
        }
    }
}
