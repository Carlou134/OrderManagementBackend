using OrderManagementBackend.Application.Dtos.Requests.Product;
using OrderManagementBackend.Application.Dtos.Responses;
using OrderManagementBackend.Application.Dtos.Responses.Common;

namespace OrderManagementBackend.Application.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<ProductDto>> GetProducts(ProductQuery query);
        Task<bool> CreateProduct(CreateProductDto request);
        Task<ProductDto?> GetProduct(int id);
        Task<bool> UpdateProduct(int id, UpdateProductDto request);
        Task<bool> DeleteProduct(int id);
    }
}
