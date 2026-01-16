using OrderManagementBackend.Application.Dtos.Requests.OrderProduct;

namespace OrderManagementBackend.Application.Dtos.Requests.Order
{
    public class UpdateOrderDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public List<UpdateOrderProductDto> Products { get; set; } = new();
    }
}
