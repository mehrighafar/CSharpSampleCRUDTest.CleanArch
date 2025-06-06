using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.IntegrationTests.Initialization;

[Collection(nameof(TestCollection))]
public class MongoDbCleanupFixture(MongoDbFixture mongoDbFixture) : IAsyncLifetime
{
  private readonly MongoDbFixture _mongoDbFixture = mongoDbFixture ?? throw new ArgumentNullException(nameof(mongoDbFixture));

  public async Task InitializeAsync()
  {
    var entities = await _mongoDbFixture.customerRepository!.GetAllAsync();
    foreach (var entity in entities!)
      await _mongoDbFixture.customerRepository!.RemoveAsync(entity.Id);
  }
  public async Task DisposeAsync()
  {
    await Task.CompletedTask;
  }
}
