namespace OrderManagementBackend.Application.Dtos.Responses
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}
