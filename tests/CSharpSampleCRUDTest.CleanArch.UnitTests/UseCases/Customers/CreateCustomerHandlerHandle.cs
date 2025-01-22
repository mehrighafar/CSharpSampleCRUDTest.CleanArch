using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.Core.Interfaces.DataAccess;
using CSharpSampleCRUDTest.CleanArch.UnitTests.Initialization;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Create;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.UnitTests.UseCases.Customers;

public class CreateCustomerHandlerHandle
{
  private readonly Mock<ICustomerDataAccessService> _customerDataAccessServiceMock = new();
  private readonly Mock<ILogger<CreateCustomerHandler>> _loggerMock = new();
  private readonly CreateCustomerHandler _handler;

  public CreateCustomerHandlerHandle()
  {
    _handler = new CreateCustomerHandler(_customerDataAccessServiceMock.Object, _loggerMock.Object);
  }

  [Fact]
  public async Task ReturnsSuccessGivenValidName()
  {
    // Arrange
    var init = new CustomerInitialization();
    init.CreateCustomers();
    var customer = init.testCustomer;

    _customerDataAccessServiceMock
        .Setup(service => service.AddAsync(It.IsAny<Customer>()))
        .ReturnsAsync(customer);

    //Act
    var result = await _handler.Handle(new CreateCustomerCommand(customer), CancellationToken.None);

    //Assert
    Assert.NotNull(result);
    Assert.Equal(customer.FirstName, result.FirstName);
  }
}
