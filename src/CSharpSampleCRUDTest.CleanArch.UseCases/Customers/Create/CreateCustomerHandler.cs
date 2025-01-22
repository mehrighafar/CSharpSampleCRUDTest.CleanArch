using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.Core.Exceptions;
using CSharpSampleCRUDTest.CleanArch.Core.Interfaces.DataAccess;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Create;

public class CreateCustomerHandler(ICustomerDataAccessService customerDataAccessService, ILogger<CreateCustomerHandler> logger) : IRequestHandler<CreateCustomerCommand, Customer>
{
  private readonly ICustomerDataAccessService _customerDataAccessService = customerDataAccessService;
  private readonly ILogger<CreateCustomerHandler> _logger = logger;

  public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
  {
    try
    {
      _logger.BeginScope("Creating a customer.");

      var result = await _customerDataAccessService.AddAsync(request.NewModel);
      return result!;
    }
    catch (Exception ex)
    when (ExceptionFilterUtility.False(() =>
    _logger!.LogError(ex, "Failed to create customer with name: {customerName}"
    , request.NewModel.FirstName)))
    {
      throw;
    }
  }
}

