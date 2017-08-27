using AutoMapper;
using System;
using Xero.Refactor.Services;
using Xero.Refactor.WebApi.Modeling;

namespace Xero.Refactor.WebApi
{

    /// <summary>
    /// Mapping profiles between API & Dto models
    /// </summary>
    public class AutoMapperApiModelProfile : Profile
    {
        public AutoMapperApiModelProfile()
        {
            // The timeStamps are converted to Base64String becase can be handled easier from the client
            CreateMap<ProductApiModel, ProductDto>()
                 .ForMember(dest => dest.RowVersion,
                opt => opt.MapFrom(src => src.RowVersion == null ? null : Convert.FromBase64String(src.RowVersion)))
                .ReverseMap()
                  .ForMember(dest => dest.RowVersion,
                opt => opt.MapFrom(src => src.RowVersion == null ? null : Convert.ToBase64String(src.RowVersion)));
            CreateMap<ProductOptionApiModel, ProductOptionDto>()
                  .ForMember(dest => dest.RowVersion,
                opt => opt.MapFrom(src => src.RowVersion == null ? null : Convert.FromBase64String(src.RowVersion)))
                .ReverseMap()
                  .ForMember(dest => dest.RowVersion,
                opt => opt.MapFrom(src => src.RowVersion == null ? null : Convert.ToBase64String(src.RowVersion)));
        }
    }
}
