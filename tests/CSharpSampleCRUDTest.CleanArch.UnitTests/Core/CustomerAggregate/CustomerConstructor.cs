using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using Xunit;

namespace CSharpSampleCRUDTest.CleanArch.UnitTests.Core.CustomerAggregate;

public class CustomerConstructor
{
  private readonly string _testName = "test name";
  private Customer? _testCustomer;

  private Customer CreateCustomer()
  {
    return new Customer { FirstName = _testName };
  }

  [Fact]
  public void InitializesName()
  {
    _testCustomer = CreateCustomer();

    Assert.Equal(_testName, _testCustomer.FirstName);
  }
}
