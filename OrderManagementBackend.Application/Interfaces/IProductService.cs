using OrderManagementBackend.Application.Dtos.Requests.Product;
using OrderManagementBackend.Application.Dtos.Responses;

namespace OrderManagementBackend.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<bool> CreateProduct(CreateProductDto request);
        Task<ProductDto?> GetProduct(int id);
        Task<bool> UpdateProduct(int id, UpdateProductDto request);
        Task<bool> DeleteProduct(int id);
    }
}
