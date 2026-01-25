using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.G02.Domain.Entities.Products;
using Store.G02.Shard.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
                //.ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => $"{configuration["BaseUrl"]}/{src.PictureUrl}"));
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(new ProductPictureUrlResolver(configuration)));

            CreateMap<ProductBrand, BrandTypeResponse>();
            CreateMap<ProductType, BrandTypeResponse>();
        }
    }
}
