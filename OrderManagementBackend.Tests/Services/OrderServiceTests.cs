using AutoMapper;
using Moq;
using OrderManagementBackend.Application.Dtos.Requests.Order;
using OrderManagementBackend.Application.Dtos.Requests.OrderProduct;
using OrderManagementBackend.Application.Services;
using OrderManagementBackend.Domain;
using OrderManagementBackend.Domain.Exceptions;
using OrderManagementBackend.Domain.Interfaces;
using OrderManagementBackend.Tests.TestHelpers;

namespace OrderManagementBackend.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepository = new();
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly IMapper _mapper = MapperFactory.Create();

        private OrderService CreateService() =>
            new(_orderRepository.Object, _productRepository.Object, _mapper);

        [Fact]
        public async Task CreateOrder_WithValidProducts_CalculatesFinalPriceFromRequestedQuantities()
        {
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Mouse", UnitPrice = 10m },
                new() { Id = 2, Name = "Keyboard", UnitPrice = 20m }
            };

            _productRepository
                .Setup(x => x.GetByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(products);

            Order? createdOrder = null;
            _orderRepository
                .Setup(x => x.CreateOrder(It.IsAny<Order>()))
                .Callback<Order>(order => createdOrder = order)
                .ReturnsAsync(true);

            var request = new CreateOrderDto
            {
                OrderNumber = "ORD1",
                Products =
                [
                    new CreateOrderProductDto { ProductId = 1, Quantity = 2 },
                    new CreateOrderProductDto { ProductId = 2, Quantity = 1 }
                ]
            };

            var result = await CreateService().CreateOrder(request);

            Assert.True(result);
            Assert.NotNull(createdOrder);
            Assert.Equal(OrderStatus.Pending, createdOrder!.Status);
            Assert.Equal(40m, createdOrder.FinalPrice);
        }

        [Fact]
        public async Task CreateOrder_WithNonExistentProduct_ThrowsBusinessRuleException()
        {
            _productRepository
                .Setup(x => x.GetByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync([new Product { Id = 1, Name = "Mouse", UnitPrice = 10m }]);

            var request = new CreateOrderDto
            {
                OrderNumber = "ORD1",
                Products =
                [
                    new CreateOrderProductDto { ProductId = 1, Quantity = 1 },
                    new CreateOrderProductDto { ProductId = 999, Quantity = 1 }
                ]
            };

            await Assert.ThrowsAsync<BusinessRuleException>(() => CreateService().CreateOrder(request));
        }

        [Fact]
        public async Task UpdateOrder_WhenOrderIsCompleted_ThrowsBusinessRuleExceptionAndDoesNotSave()
        {
            var order = new Order { Id = 1, Status = OrderStatus.Completed, OrderProducts = [] };
            _orderRepository.Setup(x => x.GetOrderById(1)).ReturnsAsync(order);

            var request = new UpdateOrderDto { OrderNumber = "ORD1", Products = [] };

            await Assert.ThrowsAsync<BusinessRuleException>(() => CreateService().UpdateOrder(1, request));

            _orderRepository.Verify(x => x.UpdateOrder(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task UpdateOrder_ExistingProduct_UpdatesQuantityInPlaceInsteadOfRecreating()
        {
            var existingItem = new OrderProduct { Id = 10, ProductId = 1, Quantity = 2, UnitPrice = 10m, TotalPrice = 20m };
            var order = new Order { Id = 1, Status = OrderStatus.Pending, OrderProducts = [existingItem] };

            _orderRepository.Setup(x => x.GetOrderById(1)).ReturnsAsync(order);
            _orderRepository.Setup(x => x.UpdateOrder(It.IsAny<Order>())).ReturnsAsync(true);
            _productRepository
                .Setup(x => x.GetByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync([new Product { Id = 1, Name = "Mouse", UnitPrice = 10m }]);

            var request = new UpdateOrderDto
            {
                OrderNumber = "ORD1",
                Products = [new UpdateOrderProductDto { ProductId = 1, Quantity = 5 }]
            };

            await CreateService().UpdateOrder(1, request);

            var resultingItem = Assert.Single(order.OrderProducts);
            Assert.Same(existingItem, resultingItem);
            Assert.Equal(5, resultingItem.Quantity);
            Assert.Equal(50m, resultingItem.TotalPrice);
        }

        [Fact]
        public async Task UpdateOrder_RemovesProductsNotInRequest_AndAddsNewOnes()
        {
            var existingItem = new OrderProduct { Id = 10, ProductId = 1, Quantity = 2, UnitPrice = 10m, TotalPrice = 20m };
            var order = new Order { Id = 1, Status = OrderStatus.Pending, OrderProducts = [existingItem] };

            _orderRepository.Setup(x => x.GetOrderById(1)).ReturnsAsync(order);
            _orderRepository.Setup(x => x.UpdateOrder(It.IsAny<Order>())).ReturnsAsync(true);
            _productRepository
                .Setup(x => x.GetByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync([new Product { Id = 2, Name = "Keyboard", UnitPrice = 20m }]);

            var request = new UpdateOrderDto
            {
                OrderNumber = "ORD1",
                Products = [new UpdateOrderProductDto { ProductId = 2, Quantity = 1 }]
            };

            await CreateService().UpdateOrder(1, request);

            var resultingItem = Assert.Single(order.OrderProducts);
            Assert.Equal(2, resultingItem.ProductId);
            Assert.NotSame(existingItem, resultingItem);
        }

        [Fact]
        public async Task DeleteOrder_WhenOrderIsCompleted_ThrowsBusinessRuleException()
        {
            var order = new Order { Id = 1, Status = OrderStatus.Completed };
            _orderRepository.Setup(x => x.GetOrderById(1)).ReturnsAsync(order);

            await Assert.ThrowsAsync<BusinessRuleException>(() => CreateService().DeleteOrder(1));

            _orderRepository.Verify(x => x.DeleteOrder(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOrder_WhenOrderIsNotCompleted_DeletesOrder()
        {
            var order = new Order { Id = 1, Status = OrderStatus.Pending };
            _orderRepository.Setup(x => x.GetOrderById(1)).ReturnsAsync(order);
            _orderRepository.Setup(x => x.DeleteOrder(1)).ReturnsAsync(true);

            var result = await CreateService().DeleteOrder(1);

            Assert.True(result);
            _orderRepository.Verify(x => x.DeleteOrder(1), Times.Once);
        }

        [Fact]
        public async Task ChangeStatus_WhenOrderIsCompleted_ThrowsBusinessRuleException()
        {
            var order = new Order { Id = 1, Status = OrderStatus.Completed };
            _orderRepository.Setup(x => x.GetOrderById(1)).ReturnsAsync(order);

            await Assert.ThrowsAsync<BusinessRuleException>(() => CreateService().ChangeStatus(OrderStatus.Pending, 1));

            _orderRepository.Verify(x => x.UpdateOrder(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task ChangeStatus_WhenOrderIsNotCompleted_UpdatesStatus()
        {
            var order = new Order { Id = 1, Status = OrderStatus.Pending };
            _orderRepository.Setup(x => x.GetOrderById(1)).ReturnsAsync(order);
            _orderRepository.Setup(x => x.UpdateOrder(It.IsAny<Order>())).ReturnsAsync(true);

            var result = await CreateService().ChangeStatus(OrderStatus.InProgress, 1);

            Assert.True(result);
            Assert.Equal(OrderStatus.InProgress, order.Status);
        }

        [Fact]
        public async Task GetOrder_WhenOrderDoesNotExist_ReturnsNull()
        {
            _orderRepository.Setup(x => x.GetOrderById(1)).ReturnsAsync((Order?)null);

            var result = await CreateService().GetOrder(1);

            Assert.Null(result);
        }
    }
}
