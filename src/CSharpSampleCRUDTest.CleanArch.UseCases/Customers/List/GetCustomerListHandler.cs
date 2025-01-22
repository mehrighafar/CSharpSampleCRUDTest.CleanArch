using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.Core.Exceptions;
using CSharpSampleCRUDTest.CleanArch.Core.Interfaces.DataAccess;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Get;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CSharpSampleCRUDTest.CleanArch.UseCases.Customers.List;

public class GetCustomerListHandler(ICustomerDataAccessService customerDataAccessService, ILogger<GetCustomerListHandler> logger) : IRequestHandler<GetCustomerListQuery, IEnumerable<Customer>?>
{
  private readonly ICustomerDataAccessService _customerDataAccessService = customerDataAccessService;
  private readonly ILogger<GetCustomerListHandler> _logger = logger;

  public async Task<IEnumerable<Customer>?> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
  {
    try
    {
      _logger.BeginScope("Getting all customers.");

      return await _customerDataAccessService.GetAllAsync();
    }
    catch (Exception ex)
    when (ExceptionFilterUtility.False(() =>
    _logger!.LogError(ex, "Failed to get all customers.")))
    {
      throw;
    }
  }
}
