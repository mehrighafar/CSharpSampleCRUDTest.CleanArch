using System.Text.Json;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.FunctionalTests.Initialization;
using CSharpSampleCRUDTest.CleanArch.Infrastructure.Repositories;
using dotenv.net;
using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.FunctionalTests.ApiEndpoints;

[Collection(nameof(MongoDbContainerCollection))]
public class CustomerList : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<MongoDbCleanupFixture>
{
  private readonly ICustomerRepository _customerRepository;
  private static string? _baseAddress;
  private readonly HttpClient? _client;
  private readonly JsonSerializerOptions _jsonSerializerOptions = new();
  private readonly List<Customer> _customerList = [];

  public CustomerList(CustomWebApplicationFactory<Program> factory, MongoDbFixture fixture)
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
  public async Task ReturnsTwoCustomers()
  {
    await AddCustomers();

    var response = await (await _client!.GetAsync("/Customer")).Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<IEnumerable<Customer>>(response, _jsonSerializerOptions);

    Assert.Equal(_customerList.Count, result!.Count());
    foreach (var customer in _customerList!)
      Assert.Contains(result!, i => i.FirstName == customer.FirstName);
  }
  private async Task AddCustomers()
  {
    var init = new CustomerInitialization();
    init.CreateCustomers();
    var customerList = init.testCustomerList;

    foreach (var customer in customerList!)
      _customerList.Add((await _customerRepository!.AddAsync(customer))!);
  }
}

