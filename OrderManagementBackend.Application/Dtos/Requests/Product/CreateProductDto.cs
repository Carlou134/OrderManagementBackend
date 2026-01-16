namespace OrderManagementBackend.Application.Dtos.Requests.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}
