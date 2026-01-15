namespace OrderManagementBackend.Application
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public double FinalPrice { get; set; }
    }

    public enum OrderStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }
}
