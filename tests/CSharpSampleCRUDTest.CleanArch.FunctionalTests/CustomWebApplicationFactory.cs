using CSharpSampleCRUDTest.CleanArch.FunctionalTests.Initialization;
using CSharpSampleCRUDTest.CleanArch.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace CSharpSampleCRUDTest.CleanArch.FunctionalTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
  private readonly MongoDbFixture _mongoDbFixture;
  public CustomWebApplicationFactory(MongoDbFixture fixture)
  {
    _mongoDbFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
  }
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.UseEnvironment("Test");

    builder.ConfigureServices(services =>
    {
      var ctx = services.SingleOrDefault(d => d.ServiceType == typeof(MongoDbExtension));
      services.Remove(ctx!);

      services.AddScoped(_ => _mongoDbFixture.testDatabase!);
    });

    var host = builder.Build();

    host.Start();
    return host;
  }
}
