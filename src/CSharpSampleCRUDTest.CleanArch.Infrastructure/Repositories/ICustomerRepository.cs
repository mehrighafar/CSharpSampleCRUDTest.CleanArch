using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using MongoDB.Driver;

namespace CSharpSampleCRUDTest.CleanArch.Infrastructure.Repositories;

public interface ICustomerRepository
{
  Task<IEnumerable<Customer>?> GetAllAsync();
  Task<Customer?> GetByIdAsync(Guid id);
  Task<Customer?> AddAsync(Customer entity);
  Task<Customer?> UpdateAsync(Customer entity);
  Task<DeleteResult> RemoveAsync(Guid id);
}
