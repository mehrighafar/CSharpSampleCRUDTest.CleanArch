using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.Core.Exceptions;
using CSharpSampleCRUDTest.CleanArch.Core.Interfaces.DataAccess;
using CSharpSampleCRUDTest.CleanArch.Infrastructure.Repositories;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CSharpSampleCRUDTest.CleanArch.Infrastructure.DataAccessServices;

public class CustomerDataAccessService
  (ICustomerRepository customerRepository, ILogger<CustomerDataAccessService> logger)
  : ICustomerDataAccessService
{
  private readonly ICustomerRepository _customerRepository = customerRepository;
  private readonly ILogger<CustomerDataAccessService> _logger = logger;
  public async Task<IEnumerable<Customer>?> GetAllAsync()
  {
    var result = await _customerRepository.GetAllAsync();
    if (result is null || !result.Any()) { return null; }

    return result;
  }

  public async Task<Customer?> GetByIdAsync(Guid id)
  {
    _logger.BeginScope("Getting customer with id: {id}.", id);

    var result = await _customerRepository.GetByIdAsync(id);

    return result is null ? throw new CustomerNotFoundException(id) : result;
  }
  public async Task<Customer?> AddAsync(Customer model)
  {
    _logger.BeginScope("Creating customer: {model}", model);

    if (!await IsEmailUniqueInDb(model.Id, model.Email))
      throw new CustomerNotUniqueEmailException(model.Email);

    else if (!await IsFirstnameLastnameDateofbirthUnique
        (model.Id, model.FirstName, model.LastName, model.DateOfBirth))
      throw new CustomerNotUniqueFirstNameLastNameDatOfBirthException
        (model.FirstName, model.LastName, model.DateOfBirth);

    var result = await _customerRepository.AddAsync(model);

    return result;
  }
  public async Task<Customer?> UpdateAsync(Customer model)
  {
    _logger.BeginScope("Updating customer: {model}", model);

    if (!await IsEmailUniqueInDb(model.Id, model.Email))
      throw new CustomerNotUniqueEmailException(model.Email);

    else if (!await IsFirstnameLastnameDateofbirthUnique
        (model.Id, model.FirstName, model.LastName, model.DateOfBirth))
      throw new CustomerNotUniqueFirstNameLastNameDatOfBirthException
        (model.FirstName, model.LastName, model.DateOfBirth);

    var result = await _customerRepository.UpdateAsync(model);

    return result;
  }
  public async Task<bool> DeleteAsync(Guid id)
  {
    _logger.BeginScope("Removing customer with id: {id}", id);

    var result = await _customerRepository.RemoveAsync(id);

    return result.IsAcknowledged ?
    result.IsAcknowledged
    : throw new Exception("An error occured while removing customer.");
  }

  private async Task<bool> IsEmailUniqueInDb(Guid id, string email)
  {
    var customers = await _customerRepository.GetAllAsync();
    if (customers is not null && customers!.Any())
    {
      var emails = customers!.Where(item => item.Id != id).Select(x => x.Email).ToList();
      if (emails!.Contains(email))
        return false;
    }

    return true;
  }

  private async Task<bool> IsFirstnameLastnameDateofbirthUnique(Guid id, string firstName, string lastName, DateOnly dateOfBirth)
  {
    var customers = await _customerRepository.GetAllAsync();
    if (customers is not null && customers!.Any())
    {
      var list = customers.Where(item => item.Id != id).Select(item => new
      {
        firstName = item.FirstName,
        lastName = item.LastName,
        dateOfBirth = item.DateOfBirth
      }).ToList();

      if (list.Contains(new { firstName, lastName, dateOfBirth }))
        return false;
    }

    return true;
  }
}
