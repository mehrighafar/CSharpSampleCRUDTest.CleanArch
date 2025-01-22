using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.FunctionalTests.Initialization;

[CollectionDefinition("MongoDbContainerCollection")]
public class MongoDbContainerCollection : ICollectionFixture<MongoDbFixture>
{
}
