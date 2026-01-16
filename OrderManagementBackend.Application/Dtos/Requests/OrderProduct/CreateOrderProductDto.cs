using System.ComponentModel.DataAnnotations;

namespace OrderManagementBackend.Application.Dtos.Requests.OrderProduct
{
    public class CreateOrderProductDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
