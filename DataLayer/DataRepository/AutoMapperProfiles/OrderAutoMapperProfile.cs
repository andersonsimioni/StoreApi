using AutoMapper;
using AutoMapper.EF6;
using StoreApi.DataLayer.DataRepository.Models;
using StoreApi.Shared.Structs.DataLayerAbstractions;

namespace StoreApi.DataLayer.DataRepository.AutoMapperProfiles;

public class OrderAutoMapperProfile : Profile
{
    public OrderAutoMapperProfile()
    {
        CreateMap<Order, OrderDTO>()
            .ForMember(dest=>dest.OrderNumber, opt=>opt.MapFrom(src=>src.Number))
            .ForMember(dest=>dest.CustomerId, opt=>opt.MapFrom(src=>src.CustomerId));
            
        CreateMap<OrderDTO, Order>()
            .ForMember(dest=>dest.Number, opt=>opt.MapFrom(src=>src.OrderNumber));

        CreateMap<CreateOrderDTO, Order>()
            .ForMember(dest=>dest.Number, opt=>opt.MapFrom(src=>src.Number))
            .ForMember(dest=>dest.CustomerId, opt=>opt.MapFrom(src=>src.CustomerId))
            .ForMember(dest=>dest.OrderItems, opt=>opt.MapFrom(src=>src.Items));

        CreateMap<Order, CreateOrderDTO>()
            .ForMember(dest=>dest.Number, opt=>opt.MapFrom(src=>src.Number))
            .ForMember(dest=>dest.CustomerId, opt=>opt.MapFrom(src=>src.CustomerId));
    }
}