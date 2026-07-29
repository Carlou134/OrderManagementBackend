using OrderManagementBackend.Application.Dtos.Requests.OrderProduct;

namespace OrderManagementBackend.Application.Dtos.Requests.Order
{
    public class CreateOrderDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public List<CreateOrderProductDto> Products { get; set; } = new();
    }
}
