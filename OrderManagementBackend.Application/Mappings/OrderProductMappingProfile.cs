using AutoMapper;
using OrderManagementBackend.Application.Dtos.Requests.OrderProduct;
using OrderManagementBackend.Application.Dtos.Responses;
using OrderManagementBackend.Domain;

namespace OrderManagementBackend.Application.Mappings
{
    public class OrderProductMappingProfile : Profile
    {
        public OrderProductMappingProfile()
        {
            CreateMap<OrderProduct, OrderProductDto>();

            CreateMap<CreateOrderProductDto, OrderProduct>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());

            CreateMap<UpdateOrderProductDto, OrderProduct>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());

        }
    }
}
