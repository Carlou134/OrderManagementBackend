using AutoMapper;
using Moq;
using OrderManagementBackend.Application.Dtos.Requests.Product;
using OrderManagementBackend.Application.Services;
using OrderManagementBackend.Domain;
using OrderManagementBackend.Domain.Exceptions;
using OrderManagementBackend.Domain.Interfaces;
using OrderManagementBackend.Tests.TestHelpers;

namespace OrderManagementBackend.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly IMapper _mapper = MapperFactory.Create();

        private ProductService CreateService() => new(_productRepository.Object, _mapper);

        [Fact]
        public async Task DeleteProduct_WhenProductIsUsedInOrders_ThrowsBusinessRuleException()
        {
            var product = new Product { Id = 1, Name = "Mouse", UnitPrice = 10m };
            _productRepository.Setup(x => x.GetProductById(1)).ReturnsAsync(product);
            _productRepository.Setup(x => x.IsProductInOrdersAsync(1)).ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessRuleException>(() => CreateService().DeleteProduct(1));

            _productRepository.Verify(x => x.DeleteProduct(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteProduct_WhenProductIsNotUsedInOrders_DeletesProduct()
        {
            var product = new Product { Id = 1, Name = "Mouse", UnitPrice = 10m };
            _productRepository.Setup(x => x.GetProductById(1)).ReturnsAsync(product);
            _productRepository.Setup(x => x.IsProductInOrdersAsync(1)).ReturnsAsync(false);
            _productRepository.Setup(x => x.DeleteProduct(1)).ReturnsAsync(true);

            var result = await CreateService().DeleteProduct(1);

            Assert.True(result);
            _productRepository.Verify(x => x.DeleteProduct(1), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_WhenProductDoesNotExist_ReturnsFalse()
        {
            _productRepository.Setup(x => x.GetProductById(1)).ReturnsAsync((Product?)null);

            var result = await CreateService().DeleteProduct(1);

            Assert.False(result);
        }

        [Fact]
        public async Task GetProduct_WhenProductDoesNotExist_ReturnsNull()
        {
            _productRepository.Setup(x => x.GetProductById(1)).ReturnsAsync((Product?)null);

            var result = await CreateService().GetProduct(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetProduct_WhenProductExists_MapsToDto()
        {
            var product = new Product { Id = 1, Name = "Mouse", UnitPrice = 10m };
            _productRepository.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

            var result = await CreateService().GetProduct(1);

            Assert.NotNull(result);
            Assert.Equal("Mouse", result!.Name);
            Assert.Equal(10m, result.UnitPrice);
        }

        [Fact]
        public async Task CreateProduct_MapsRequestAndCallsRepository()
        {
            Product? createdProduct = null;
            _productRepository
                .Setup(x => x.CreateProduct(It.IsAny<Product>()))
                .Callback<Product>(p => createdProduct = p)
                .ReturnsAsync(true);

            var request = new CreateProductDto { Name = "Monitor", UnitPrice = 150m };

            var result = await CreateService().CreateProduct(request);

            Assert.True(result);
            Assert.NotNull(createdProduct);
            Assert.Equal("Monitor", createdProduct!.Name);
            Assert.Equal(150m, createdProduct.UnitPrice);
        }
    }
}
