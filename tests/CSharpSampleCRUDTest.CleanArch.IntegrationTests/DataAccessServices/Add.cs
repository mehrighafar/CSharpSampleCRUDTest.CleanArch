using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.Infrastructure.Repositories;
using CSharpSampleCRUDTest.CleanArch.IntegrationTests.Initialization;
using MongoDB.Driver;
using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.IntegrationTests.DataAccessServices;

[Collection(nameof(TestCollection))]
public class Add(MongoDbFixture fixture)
{
  private readonly MongoCustomerRepository _repository = fixture.customerRepository!;

  [Fact]
  public async Task AddsCustomerAndSetsId()
  {
    // Arrange
    var init = new CustomerInitialization();
    init.CreateCustomers();
    var customer = init.testCustomer;
    Customer? newCustomer = null;

    //Act
    await _repository!.AddAsync(customer);

    var customers = await _repository!.GetAllAsync();
    if (customers is not null)
      newCustomer = customers!.FirstOrDefault();

    //Assert
    Assert.Equal(customer.FirstName, newCustomer?.FirstName);
  }
}
