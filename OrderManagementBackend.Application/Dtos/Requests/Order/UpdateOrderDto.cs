using OrderManagementBackend.Application.Dtos.Requests.OrderProduct;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementBackend.Application.Dtos.Requests.Order
{
    public class UpdateOrderDto
    {
        [Required]
        [MaxLength(10)]
        public string OrderNumber { get; set; } = string.Empty;
        [Required]
        public List<UpdateOrderProductDto> Products { get; set; } = new();
    }
}
