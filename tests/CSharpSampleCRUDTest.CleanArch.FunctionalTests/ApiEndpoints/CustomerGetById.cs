using System.Text.Json;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.FunctionalTests.Initialization;
using CSharpSampleCRUDTest.CleanArch.Infrastructure.Repositories;
using dotenv.net;
using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.FunctionalTests.ApiEndpoints;

[Collection(nameof(MongoDbContainerCollection))]
public class CustomerGetById : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<MongoDbCleanupFixture>
{
  private readonly ICustomerRepository _customerRepository;
  private static string? _baseAddress;
  private readonly HttpClient? _client;
  private readonly JsonSerializerOptions _jsonSerializerOptions = new();
  private readonly List<Customer> _customerList = [];

  public CustomerGetById(CustomWebApplicationFactory<Program> factory, MongoDbFixture fixture)
  {
    _jsonSerializerOptions = new JsonSerializerOptions
    {
      AllowTrailingCommas = true,
      PropertyNameCaseInsensitive = true,
      WriteIndented = false
    };

    _customerRepository = fixture.customerRepository!;

    DotEnv.Load(new DotEnvOptions(envFilePaths: ["../../../.env"]));

    // HTTP client
    _baseAddress = Environment.GetEnvironmentVariable("API_BASE_ADDRESS")!
      ?? throw new InvalidOperationException("Api address for test is not set.");
    _client = factory.CreateDefaultClient(new Uri(_baseAddress!));
  }
  [Fact]
  public async Task ReturnsAddedCustomerGivenId1()
  {
    await AddCustomer();

    var response = await (await _client!.GetAsync
      ($"/Customer/{_customerList!.FirstOrDefault()!.Id}")).Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<Customer>(response, _jsonSerializerOptions);

    Assert.Equal(_customerList!.FirstOrDefault()!.Id, result!.Id);
    Assert.Equal(_customerList!.FirstOrDefault()!.FirstName, result.FirstName);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenId1000()
  {
    var response = await _client!.GetAsync("/Customer/1000");
    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
  }
  private async Task AddCustomer()
  {
    var init = new CustomerInitialization();
    init.CreateCustomers();
    var customer = init.testCustomer;

    _customerList.Add((await _customerRepository!.AddAsync(customer!))!);
  }
}
