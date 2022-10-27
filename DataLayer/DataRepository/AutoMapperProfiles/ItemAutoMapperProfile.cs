using AutoMapper;
using AutoMapper.EF6;
using StoreApi.DataLayer.DataRepository.Models;
using StoreApi.Shared.Structs.DataLayerAbstractions;

namespace StoreApi.DataLayer.DataRepository.AutoMapperProfiles;

public class ItemAutoMapperProfile : Profile
{
    public ItemAutoMapperProfile()
    {
        CreateMap<Item, ItemDTO>();
        CreateMap<ItemDTO, Item>();

        CreateMap<CreateItemDTO, Item>();
        CreateMap<Item, CreateItemDTO>();

        CreateMap<Item, OrderItem>()
            .ForMember(dest=>dest.Id, opt=>opt.MapFrom(src=>0));
    }
}