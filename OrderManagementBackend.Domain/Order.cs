namespace OrderManagementBackend.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; }
        public decimal FinalPrice { get; set; }
        public ICollection<OrderProduct> OrderProduct { get; set; } = new List<OrderProduct>();
    }

    public enum OrderStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }
}
