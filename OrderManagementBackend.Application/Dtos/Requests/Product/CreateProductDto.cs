using System.ComponentModel.DataAnnotations;

namespace OrderManagementBackend.Application.Dtos.Requests.Product
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal UnitPrice { get; set; }
    }
}
