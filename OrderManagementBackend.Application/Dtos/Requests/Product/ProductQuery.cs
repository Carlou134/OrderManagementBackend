using OrderManagementBackend.Application.Dtos.Requests.Common;

namespace OrderManagementBackend.Application.Dtos.Requests.Product
{
    public class ProductQuery : PaginationQuery
    {
        public string? Name { get; set; }
    }
}
