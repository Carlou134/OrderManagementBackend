using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using OrderManagementBackend.Application.Mappings;

namespace OrderManagementBackend.Tests.TestHelpers
{
    public static class MapperFactory
    {
        public static IMapper Create()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OrderMappingProfile>();
                cfg.AddProfile<ProductMappingProfile>();
                cfg.AddProfile<OrderProductMappingProfile>();
            }, NullLoggerFactory.Instance);

            return configuration.CreateMapper();
        }
    }
}
