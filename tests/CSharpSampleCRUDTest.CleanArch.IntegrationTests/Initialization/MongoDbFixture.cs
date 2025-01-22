using CSharpSampleCRUDTest.CleanArch.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Serilog;
using Serilog.Extensions.Logging;
using Testcontainers.MongoDb;
using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.IntegrationTests.Initialization;
public class MongoDbFixture : IAsyncLifetime
{
  private MongoDbContainer? _mongoDbContainer;
  public MongoCustomerRepository? customerRepository;
  private static bool _serializersRegistered = false;

  public async Task InitializeAsync()
  {
    _mongoDbContainer = new MongoDbBuilder().WithName("test-mongo").WithImage("mongo:latest")
      .WithReuse(true).Build();

    if (!_serializersRegistered)
    {
      BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
      _serializersRegistered = true;
    }

    await _mongoDbContainer.StartAsync();
    var client = new MongoClient(_mongoDbContainer!.GetConnectionString());
    var testDatabase = client.GetDatabase("test");

    //
    Log.Logger = new LoggerConfiguration()
           .WriteTo.TestCorrelator()
           .CreateLogger();

    var loggerFactory = new SerilogLoggerFactory(Log.Logger);
    var logger = loggerFactory.CreateLogger<MongoCustomerRepository>();

    customerRepository = new MongoCustomerRepository(testDatabase, logger);
  }

  public async Task DisposeAsync()
  {
    await _mongoDbContainer!.DisposeAsync();
  }
}

