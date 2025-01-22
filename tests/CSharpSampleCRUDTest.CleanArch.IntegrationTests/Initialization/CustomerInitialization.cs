using System.Text.Json;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.UnitTests.Initialization.Helpers;
using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.IntegrationTests.Initialization;
public class CustomerInitialization
{
  private JsonFilesRepository _jsonFilesRepo = new();
  private JsonSerializerOptions _jsonSerializerOptions;
  public Customer testCustomer = new();
  public IList<Customer>? testCustomerList = null;

  public CustomerInitialization()
  {
    _jsonSerializerOptions = new JsonSerializerOptions
    {
      AllowTrailingCommas = true,
      PropertyNameCaseInsensitive = true,
      WriteIndented = false
    };
  }

  public void CreateCustomers()
  {
    var testCustomerJson = _jsonFilesRepo.Files["customer.json"];
    testCustomer = JsonSerializer.Deserialize<Customer>(testCustomerJson, _jsonSerializerOptions)!;

    var testCustomerListJson = _jsonFilesRepo.Files["customers.json"];
    testCustomerList = JsonSerializer.Deserialize<IList<Customer>>(testCustomerListJson, _jsonSerializerOptions)!;
  }
}
