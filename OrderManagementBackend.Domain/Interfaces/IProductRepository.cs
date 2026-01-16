namespace OrderManagementBackend.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyCollection<Product>> ListProducts();
        Task<bool> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);
        Task<Product?> GetProductById(int id);
        Task<bool> IsProductInOrdersAsync(int productId);
        Task<IReadOnlyCollection<Product>> GetByIdsAsync(List<int> ids);
    }
}
