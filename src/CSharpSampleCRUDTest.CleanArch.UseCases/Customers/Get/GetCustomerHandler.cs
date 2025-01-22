using System.Threading;
using System.Threading.Tasks;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.Core.Exceptions;
using CSharpSampleCRUDTest.CleanArch.Core.Interfaces.DataAccess;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Delete;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Get;

public class GetCustomerHandler(ICustomerDataAccessService customerDataAccessService, ILogger<GetCustomerHandler> logger) : IRequestHandler<GetCustomerQuery, Customer>
{
  private readonly ICustomerDataAccessService _customerDataAccessService = customerDataAccessService;
  private readonly ILogger<GetCustomerHandler> _logger = logger;

  public async Task<Customer> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
  {
    try
    {
      _logger.BeginScope("Getting a customer.");

      var result = await _customerDataAccessService.GetByIdAsync(request.Id);

      return result!;
    }
    catch (Exception ex)
    when (ExceptionFilterUtility.False(() =>
    _logger!.LogError(ex, "Failed to get customer with id :{id}.", request.Id)))
    {
      throw;
    }
  }
}

