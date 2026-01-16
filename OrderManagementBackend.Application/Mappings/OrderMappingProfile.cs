using AutoMapper;
using OrderManagementBackend.Application.Dtos.Requests.Order;
using OrderManagementBackend.Application.Dtos.Responses;
using OrderManagementBackend.Domain;

namespace OrderManagementBackend.Application.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.NumberProducts, opt => opt.MapFrom(x => x.OrderProducts.Count));

            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.FinalPrice, opt => opt.Ignore());

            CreateMap<UpdateOrderDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.FinalPrice, opt => opt.Ignore());
        }
    }
}
