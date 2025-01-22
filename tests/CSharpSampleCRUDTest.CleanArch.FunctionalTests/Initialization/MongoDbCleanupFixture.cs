using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URF.Core.Abstractions;
using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.FunctionalTests.Initialization;

[Collection(nameof(MongoDbContainerCollection))]
public class MongoDbCleanupFixture : IAsyncLifetime
{
  private readonly MongoDbFixture _mongoDbFixture;
  public MongoDbCleanupFixture(MongoDbFixture mongoDbFixture)
  {
    _mongoDbFixture = mongoDbFixture ?? throw new ArgumentNullException(nameof(mongoDbFixture));
  }
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
