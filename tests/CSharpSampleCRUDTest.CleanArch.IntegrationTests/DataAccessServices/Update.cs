using CSharpSampleCRUDTest.CleanArch.Infrastructure.Repositories;
using CSharpSampleCRUDTest.CleanArch.IntegrationTests.Initialization;
using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.IntegrationTests.DataAccessServices;

[Collection(nameof(TestCollection))]
public class Update(MongoDbFixture fixture)
{
  private readonly MongoCustomerRepository _repository = fixture.customerRepository!;

  [Fact]
  public async Task UpdatesItemAfterAddingIt()
  {
    // Arrange
    var init = new CustomerInitialization();
    init.CreateCustomers();
    var customer = init.testCustomer;

    var addedCustomer = await _repository!.AddAsync(customer);

    // fetch the item and update its FirstName
    var newCustomer = (await _repository.GetAllAsync())!
      .FirstOrDefault(customer => customer.Id == addedCustomer!.Id);

    if (newCustomer == null)
    {
      Assert.NotNull(newCustomer);
      return;
    }
    Assert.NotSame(addedCustomer, newCustomer);
    var newName = Guid.NewGuid().ToString();
    newCustomer.FirstName = newName;

    // Act
    await _repository.UpdateAsync(newCustomer);

    var updatedItem = (await _repository.GetAllAsync())!
      .FirstOrDefault(customer => customer.FirstName == newName);

    // Assert
    Assert.NotNull(updatedItem);
    Assert.NotEqual(addedCustomer!.FirstName, updatedItem?.FirstName);
    Assert.Equal(addedCustomer.Id, updatedItem?.Id);
    Assert.Equal(addedCustomer.LastName, updatedItem?.LastName);
    Assert.Equal(addedCustomer.PhoneNumber, updatedItem?.PhoneNumber);
    Assert.Equal(addedCustomer.BankAccountNumber, updatedItem?.BankAccountNumber);
    Assert.Equal(addedCustomer.DateOfBirth, updatedItem?.DateOfBirth);
    Assert.Equal(addedCustomer.Email, updatedItem?.Email);
  }
}
