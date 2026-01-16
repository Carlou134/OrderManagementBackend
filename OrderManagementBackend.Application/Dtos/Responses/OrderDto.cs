using OrderManagementBackend.Domain;

namespace OrderManagementBackend.Application.Dtos.Responses
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; }
        public decimal FinalPrice { get; set; }
        public int NumberProducts { get; set; }
        public ICollection<OrderProductDto>? OrderProducts { get; set; }
    }
}
