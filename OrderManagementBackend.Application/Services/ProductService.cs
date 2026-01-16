using AutoMapper;
using OrderManagementBackend.Application.Dtos.Requests.Product;
using OrderManagementBackend.Application.Dtos.Responses;
using OrderManagementBackend.Application.Interfaces;
using OrderManagementBackend.Domain;
using OrderManagementBackend.Domain.Interfaces;

namespace OrderManagementBackend.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var result = await _repository.ListProducts();
            return _mapper.Map<IEnumerable<ProductDto>>(result);
        }
        public async Task<bool> CreateProduct(CreateProductDto request)
        {
            var newProduct = _mapper.Map<Product>(request);
            return await _repository.CreateProduct(newProduct);
        }

        public async Task<ProductDto?> GetProduct(int id)
        {
            var product = await _repository.GetProductById(id);

            if (product != null)
            {
                return _mapper.Map<ProductDto>(product);
            }

            return null;
        }

        public async Task<bool> UpdateProduct(int id, UpdateProductDto request)
        {
            var product = await _repository.GetProductById(id);

            if (product != null)
            {
                _mapper.Map(request, product);
                return await _repository.UpdateProduct(product);
            }

            return false;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _repository.GetProductById(id);

            if (product != null)
            {
                var isInOrders = await _repository.IsProductInOrdersAsync(id);

                if (isInOrders) throw new InvalidOperationException("Cannot delete product because it is used in existing orders");

                return await _repository.DeleteProduct(id);
            }

            return false;
        }
    }
}
