using OrderManagementBackend.Domain.Common;

namespace OrderManagementBackend.Domain
{
    public class Order : IAuditableEntity
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; }
        public decimal FinalPrice { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
    }

    public enum OrderStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }
}
