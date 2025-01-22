using CSharpSampleCRUDTest.CleanArch.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
namespace CSharpSampleCRUDTest.CleanArch.Infrastructure.Extensions;

public static class RepositoryExtension
{
  public static IServiceCollection AddRepository(this IServiceCollection services)
  {
    // Add Customer collection
    services.AddScoped<ICustomerRepository, MongoCustomerRepository>();
    return services;
  }
}

