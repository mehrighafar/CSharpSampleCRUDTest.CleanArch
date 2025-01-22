using AutoMapper;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers;

namespace CSharpSampleCRUDTest.CleanArch.Web.MapperProfiles;

public class UpdateCustomerDtoANDCustomerMapperProfile : Profile
{
  public UpdateCustomerDtoANDCustomerMapperProfile()
  {
    CreateMap<Customer, UpdateCustomerDTO>().ReverseMap();
  }
}
