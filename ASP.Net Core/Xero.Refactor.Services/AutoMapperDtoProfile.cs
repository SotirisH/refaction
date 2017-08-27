using AutoMapper;
using Xero.Refactor.Data.Models;

namespace Xero.Refactor.Services
{

    /// <summary>
    /// Mapping profiles between EF & Dto models
    /// On the .net core every class that inherits Profile is automatically registered
    /// </summary>
    public class AutoMapperDtoProfile : Profile
    {
        public AutoMapperDtoProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductOption, ProductOptionDto>().ReverseMap();
        }
    }
}
