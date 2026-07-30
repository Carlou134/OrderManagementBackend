using Microsoft.EntityFrameworkCore;
using OrderManagementBackend.Domain;
using OrderManagementBackend.Domain.Interfaces;
using OrderManagementBackend.Infrastructure.Data;

namespace OrderManagementBackend.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersContext _context;

        public OrderRepository(OrdersContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyCollection<Order> Items, int TotalCount)> ListOrders(OrderStatus? status, int page, int pageSize)
        {
            var query = _context.Order.Include(x => x.OrderProducts).AsNoTracking().AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> CreateOrder(Order order)
        {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _context.Order.Include(x => x.OrderProducts).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            _context.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await GetOrderById(id);

            if (order != null)
            {
                _context.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> IsOrderInOrdersAsync(int orderId)
        {
            return await _context.OrderProduct.AnyAsync(x => x.OrderId == orderId);
        }
    }
}
