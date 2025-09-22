using AutoMapper;
using IPLMerch.Application.DTOs;
using IPLMerch.Application.Models;

namespace IPLMerch.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
            
        CreateMap<Franchise, FranchiseDto>();
            
        CreateMap<Cart, CartDto>();
            
        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.Quantity * src.Product.Price));
            
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        
        CreateMap<OrderItem, OrderItemDto>();
    }
}