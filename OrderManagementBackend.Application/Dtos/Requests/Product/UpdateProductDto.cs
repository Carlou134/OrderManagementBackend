namespace OrderManagementBackend.Application.Dtos.Requests.Product
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}
