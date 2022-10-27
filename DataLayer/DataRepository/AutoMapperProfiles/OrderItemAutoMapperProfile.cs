using AutoMapper;
using AutoMapper.EF6;
using StoreApi.DataLayer.DataRepository.Models;
using StoreApi.Shared.Structs.DataLayerAbstractions;

namespace StoreApi.DataLayer.DataRepository.AutoMapperProfiles;

public class OrderItemAutoMapperProfile : Profile
{
    public OrderItemAutoMapperProfile()
    {
        CreateMap<CreateOrderDTO.OrderItemDTO, OrderItem>();
    }
}