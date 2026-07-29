namespace OrderManagementBackend.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<(IReadOnlyCollection<Order> Items, int TotalCount)> ListOrders(OrderStatus? status, int page, int pageSize);
        Task<bool> CreateOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(int id);
        Task<Order?> GetOrderById(int id);
        Task<bool> IsOrderInOrdersAsync(int orderId);
    }
}
