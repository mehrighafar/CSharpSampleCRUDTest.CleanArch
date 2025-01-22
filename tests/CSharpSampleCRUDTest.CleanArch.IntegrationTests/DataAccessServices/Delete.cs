using CSharpSampleCRUDTest.CleanArch.Infrastructure.Repositories;
using CSharpSampleCRUDTest.CleanArch.IntegrationTests.Initialization;
using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.IntegrationTests.DataAccessServices;

[Collection(nameof(TestCollection))]
public class Delete(MongoDbFixture fixture)
{
  private readonly MongoCustomerRepository _repository = fixture.customerRepository!;

  [Fact]
  public async Task DeletesItemAfterAddingIt()
  {
    // Arrange
    var init = new CustomerInitialization();
    init.CreateCustomers();
    var customer = init.testCustomer;

    var addedCustomer = await _repository!.AddAsync(customer);

    // Act
    await _repository.RemoveAsync(addedCustomer!.Id);

    //Assert
    var result = await _repository!.GetAllAsync();
    Assert.DoesNotContain((await _repository!.GetAllAsync())!,
      customer => customer.Id == addedCustomer.Id);
  }
}
