using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.IntegrationTests.Initialization;

[CollectionDefinition("TestCollection")]
public class TestCollection : ICollectionFixture<MongoDbFixture>
{
}
