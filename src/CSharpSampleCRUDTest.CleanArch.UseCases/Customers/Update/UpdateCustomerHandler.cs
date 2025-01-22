using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.Core.Exceptions;
using CSharpSampleCRUDTest.CleanArch.Core.Interfaces.DataAccess;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Update;

public class UpdateCustomerHandler(ICustomerDataAccessService customerDataAccessService, ILogger<UpdateCustomerHandler> logger) : IRequestHandler<UpdateCustomerCommand, Customer>
{
  private readonly ICustomerDataAccessService _customerDataAccessService = customerDataAccessService;
  private readonly ILogger<UpdateCustomerHandler> _logger = logger;

  public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
  {
    try
    {
      _logger.BeginScope("Updating a customer.");

      return (await _customerDataAccessService.UpdateAsync(request.NewModel))!;
    }
    catch (Exception ex)
   when (ExceptionFilterUtility.False(() =>
   _logger!.LogError(ex, "Failed to update customer with id :{id}.", request.NewModel.Id)))
    {
      throw;
    }
  }
}
