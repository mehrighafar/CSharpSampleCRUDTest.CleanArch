using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;

namespace CSharpSampleCRUDTest.CleanArch.Core.Interfaces.DataAccess;

public interface ICustomerDataAccessService
{
  public Task<IEnumerable<Customer>?> GetAllAsync();
  public Task<Customer?> GetByIdAsync(Guid id);
  public Task<Customer?> AddAsync(Customer model);
  public Task<Customer?> UpdateAsync(Customer model);
  public Task<bool> DeleteAsync(Guid id);
}

