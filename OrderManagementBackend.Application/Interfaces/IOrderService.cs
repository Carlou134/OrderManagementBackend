using OrderManagementBackend.Application.Dtos.Requests.Order;
using OrderManagementBackend.Application.Dtos.Responses;
using OrderManagementBackend.Domain;

namespace OrderManagementBackend.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrders();
        Task<bool> CreateOrder(CreateOrderDto request);
        Task<OrderDto?> GetOrder(int id);
        Task<bool> UpdateOrder(int id, UpdateOrderDto request);
        Task<bool> DeleteOrder(int id);
        Task<bool> ChangeStatus(OrderStatus status, int id);
    }
}
