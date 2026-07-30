using OrderManagementBackend.Application.Dtos.Requests.Common;
using OrderManagementBackend.Domain;

namespace OrderManagementBackend.Application.Dtos.Requests.Order
{
    public class OrderQuery : PaginationQuery
    {
        public OrderStatus? Status { get; set; }
    }
}
