using AutoMapper;
using OrderManagementBackend.Application.Dtos.Requests.Order;
using OrderManagementBackend.Application.Dtos.Responses;
using OrderManagementBackend.Application.Interfaces;
using OrderManagementBackend.Domain;
using OrderManagementBackend.Domain.Interfaces;

namespace OrderManagementBackend.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IProductRepository productRepository, IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetOrders()
        {
            var result = await _repository.ListOrders();
            return _mapper.Map<IEnumerable<OrderDto>>(result);
        }

        public async Task<bool> CreateOrder(CreateOrderDto request)
        {
            var productsIds = request.Products.Select(x => x.ProductId).ToList();
            var products = await _productRepository.GetByIdsAsync(productsIds);

            if(products.Count != productsIds.Count)
            {
                throw new InvalidOperationException("One or more products not found");
            }

            var order = new Order
            {
                OrderNumber = request.OrderNumber,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                OrderProducts = request.Products.Select(x =>
                {
                    var product = products.First(pr => pr.Id == x.ProductId);

                    return new OrderProduct
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        UnitPrice = product.UnitPrice,
                        TotalPrice = x.Quantity * product.UnitPrice
                    };
                }).ToList()
            };

            order.FinalPrice = order.OrderProducts.Sum(x => x.TotalPrice);

            return await _repository.CreateOrder(order);
        }

        public async Task<OrderDto?> GetOrder(int id)
        {
            var order = await _repository.GetOrderById(id);

            if (order == null)
                return null;

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<bool> UpdateOrder(int id, UpdateOrderDto request)
        {
            var order = await _repository.GetOrderById(id);

            if (order != null)
            {
                if(order.Status == OrderStatus.Completed)
                {
                    throw new InvalidOperationException("Cannot modify completed orders");
                }

                var productsIds = request.Products.Select(x => x.ProductId).ToList();
                var products = await _productRepository.GetByIdsAsync(productsIds);

                if (products.Count != productsIds.Count)
                {
                    throw new InvalidOperationException("One or more products not found");
                }

                order.OrderProducts.Clear();
                order.OrderProducts = request.Products.Select(x =>
                {
                    var product = products.First(pr => pr.Id == x.ProductId);

                    return new OrderProduct
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        UnitPrice = product.UnitPrice,
                        TotalPrice = x.Quantity * product.UnitPrice
                    };
                }).ToList();

                order.OrderNumber = request.OrderNumber;
                order.FinalPrice = order.OrderProducts.Sum(x => x.TotalPrice);
                return await _repository.UpdateOrder(order);
            }

            return false;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _repository.GetOrderById(id);

            if (order != null)
            {
                var isInOrders = await _repository.IsOrderInOrdersAsync(id);

                if (isInOrders) throw new InvalidOperationException("Cannot delete order because it is used in existing orders");

                return await _repository.DeleteOrder(id);
            }

            return false;
        }

        public async Task<bool> ChangeStatus(OrderStatus status, int id)
        {
            var order = await _repository.GetOrderById(id);

            if (order != null)
            {
                order.Status = status;
                return await _repository.UpdateOrder(order);
            }

            return false;
        }

    }
}
