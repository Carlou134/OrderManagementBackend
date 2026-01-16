using OrderManagementBackend.Domain;

namespace OrderManagementBackend.Application.Dtos.Requests.Order
{
    public class ChangeOrderStatusDto
    {
        public OrderStatus status {  get; set; }
    }
}
