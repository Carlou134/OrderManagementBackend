using OrderManagementBackend.Application.Dtos.Requests.OrderProduct;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementBackend.Application.Dtos.Requests.Order
{
    public class CreateOrderDto
    {
        [Required]
        [MaxLength(10)]
        public string OrderNumber { get; set; } = string.Empty;
        [Required]
        public List<CreateOrderProductDto> Products { get; set; } = new();
    }
}
