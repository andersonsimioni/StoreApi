using AutoMapper;
using AutoMapper.EF6;
using StoreApi.DataLayer.DataRepository.Models;
using StoreApi.Shared.Structs.ApiDTOs;
using StoreApi.Shared.Structs.DataLayerAbstractions;

namespace StoreApi.DataLayer.DataRepository.AutoMapperProfiles;

public class CustomerAutoMapperProfile : Profile
{
    public CustomerAutoMapperProfile()
    {
        CreateMap<Customer, CustomerDTO>();
        CreateMap<CustomerDTO, Customer>();
        
        CreateMap<CreateCustomerDTO, Customer>();
        CreateMap<Customer, CreateCustomerDTO>();
    }
}