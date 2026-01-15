namespace OrderManagementBackend.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public ICollection<OrderProduct> OrderProduct { get; set; } = new List<OrderProduct>();
    }
}
