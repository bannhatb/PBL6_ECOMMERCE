using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelDtos;

namespace Website_Ecommerce.API.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Source -> target
              CreateMap<ProductDetailDto, ProductDetail>();
              CreateMap<ProductDetail, ProductDetailDto>();

              CreateMap<ProductImageDto, ProductImage>();
              CreateMap<ProductImage, ProductImageDto>();

              CreateMap<CartDto, Cart>()
                     .ForMember(dest => dest.State, opt => opt.Ignore())
                     .ForMember(dest => dest.UserId, opt => opt.Ignore());

              CreateMap<VoucherOrderDto, VoucherOrder>();

              CreateMap<VoucherShopDto, VoucherProduct>();

              CreateMap<VoucherProduct, VoucherShopDto>();

              CreateMap<User, ShipperDto>();

              CreateMap<User, ProfileDto>();

              CreateMap<ProfileDto, User>()
                     .ForMember(dest => dest.Username, opt => opt.Ignore())
                     .ForMember(dest => dest.Password, opt => opt.Ignore());

              CreateMap<ShopDto, Shop>()
                     .ForMember(dest => dest.Email, opt => opt.Ignore())
                     .ForMember(dest => dest.Status, opt => opt.Ignore())
                     .ForMember(dest => dest.TotalRate, opt => opt.Ignore())
                     .ForMember(dest => dest.AverageRate, opt => opt.Ignore())
                     .ForMember(dest => dest.UserId, opt => opt.Ignore());

              CreateMap<CommentDto, Comment>()
                     .ForMember(dest => dest.State, opt => opt.Ignore())
                     .ForMember(dest => dest.UserId, opt => opt.Ignore())
                     .ForMember(dest => dest.ProductId, opt => opt.Ignore());

              CreateMap<ProductDto, Product>()
                     .ForMember(dest => dest.TotalRate, opt => opt.Ignore());




        }
    }
}