using AutoMapper;
using OrderManagementBackend.Application.Dtos.Requests.Product;
using OrderManagementBackend.Application.Dtos.Responses;
using OrderManagementBackend.Application.Dtos.Responses.Common;
using OrderManagementBackend.Application.Interfaces;
using OrderManagementBackend.Domain;
using OrderManagementBackend.Domain.Exceptions;
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

        public async Task<PagedResult<ProductDto>> GetProducts(ProductQuery query)
        {
            var (items, totalCount) = await _repository.ListProducts(query.Name, query.Page, query.PageSize);

            return new PagedResult<ProductDto>
            {
                Items = _mapper.Map<IReadOnlyCollection<ProductDto>>(items),
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
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

                if (isInOrders) throw new BusinessRuleException("Cannot delete product because it is used in existing orders");

                return await _repository.DeleteProduct(id);
            }

            return false;
        }
    }
}
