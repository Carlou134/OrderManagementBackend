using AutoMapper;
using OrderManagementBackend.Application.Dtos.Requests.Product;
using OrderManagementBackend.Application.Dtos.Responses;
using OrderManagementBackend.Domain;

namespace OrderManagementBackend.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>();

            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateProductDto, Product>();
        }
    }
}
