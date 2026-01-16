using System.ComponentModel.DataAnnotations;

namespace OrderManagementBackend.Application.Dtos.Requests.OrderProduct
{
    public class UpdateOrderProductDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
