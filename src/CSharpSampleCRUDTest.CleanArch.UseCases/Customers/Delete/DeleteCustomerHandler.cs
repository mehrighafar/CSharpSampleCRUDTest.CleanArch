using CSharpSampleCRUDTest.CleanArch.Core.Exceptions;
using CSharpSampleCRUDTest.CleanArch.Core.Interfaces.DataAccess;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Create;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Delete;

public class DeleteCustomerHandler(ICustomerDataAccessService customerDataAccessService, ILogger<DeleteCustomerHandler> logger) : IRequestHandler<DeleteCustomerCommand, bool>
{
  private readonly ICustomerDataAccessService _customerDataAccessService = customerDataAccessService;
  private readonly ILogger<DeleteCustomerHandler> _logger = logger;

  public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
  {
    try
    {
      _logger.BeginScope("Removing a customer.");

      var result = await _customerDataAccessService.DeleteAsync(request.Id);

      return result;
    }
    catch (Exception ex)
    when (ExceptionFilterUtility.False(() =>
    _logger!.LogError(ex, "Failed to remove customer with id :{id}.", request.Id)))
    {
      throw;
    }
  }
}

