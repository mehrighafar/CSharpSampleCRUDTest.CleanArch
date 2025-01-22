using AutoMapper;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers;

namespace CSharpSampleCRUDTest.CleanArch.Web.MapperProfiles;

public class CustomerDtoANDCustomerMapperProfile : Profile
{
  public CustomerDtoANDCustomerMapperProfile()
  {
    CreateMap<Customer, CustomerDTO>().ReverseMap();
  }
}
