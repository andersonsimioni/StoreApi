using AutoMapper;

namespace StoreApi.DataLayer.DataRepository.AutoMapperProfiles;

public class AutoMapperConfiguration
{
    public static MapperConfiguration? Create()
    {
        var configuration = new MapperConfiguration(cfg => {
            cfg.AddProfile<OrderItemAutoMapperProfile>();
            cfg.AddProfile<CustomerAutoMapperProfile>();
            cfg.AddProfile<OrderAutoMapperProfile>();
            cfg.AddProfile<ItemAutoMapperProfile>();
        });


        return configuration;
    }
}