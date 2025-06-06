using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.Core.Exceptions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CSharpSampleCRUDTest.CleanArch.Infrastructure.Repositories;

public class MongoCustomerRepository
  (IMongoDatabase mongoDatabase, ILogger<MongoCustomerRepository> logger)
  : ICustomerRepository
{
  private readonly IMongoCollection<Customer> _mongoCollection = mongoDatabase.GetCollection<Customer>(nameof(Customer));
  private readonly ILogger<MongoCustomerRepository> _logger = logger;

  public async Task<IEnumerable<Customer>?> GetAllAsync()
  {
    _logger.BeginScope("Getting all customers from repository.");

    var filter = Builders<Customer>.Filter.Empty;
    return await _mongoCollection.Find(filter).ToListAsync();
  }

  public async Task<Customer?> GetByIdAsync(Guid id)
  {
    _logger.BeginScope("Getting customer from repository with id: {id}.", id);

    return await _mongoCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
  }

  public async Task<Customer?> AddAsync(Customer entity)
  {
    _logger.BeginScope("Creating customer: {entity} in repository.", entity);

    if (!await IsEmailUniqueInDb(entity.Id, entity.Email))
      throw new CustomerNotUniqueEmailException(entity.Email);

    else if (!await IsFirstnameLastnameDateofbirthUnique
        (entity.Id, entity.FirstName, entity.LastName, entity.DateOfBirth))
      throw new CustomerNotUniqueFirstNameLastNameDatOfBirthException
        (entity.FirstName, entity.LastName, entity.DateOfBirth);

    entity.Id = Guid.NewGuid();
    await _mongoCollection.InsertOneAsync(entity);
    return entity;
  }

  public async Task<Customer?> UpdateAsync(Customer entity)
  {
    _logger.BeginScope("Updating customer: {entity} in repository.", entity);

    if (!await IsEmailUniqueInDb(entity.Id, entity.Email))
      throw new CustomerNotUniqueEmailException(entity.Email);

    else if (!await IsFirstnameLastnameDateofbirthUnique
        (entity.Id, entity.FirstName, entity.LastName, entity.DateOfBirth))
      throw new CustomerNotUniqueFirstNameLastNameDatOfBirthException
        (entity.FirstName, entity.LastName, entity.DateOfBirth);

    return await _mongoCollection.FindOneAndReplaceAsync(e => e.Id == entity.Id, entity);
  }

  public async Task<DeleteResult> RemoveAsync(Guid id)
  {
    _logger.BeginScope("Removing customer from repository with id: {id}.", id);

    return await _mongoCollection.DeleteOneAsync(e => e.Id == id);
  }

  private async Task<bool> IsEmailUniqueInDb(Guid id, string email)
  {
    var filter =
        Builders<Customer>.Filter.Ne(u => u.Id, id) &
        Builders<Customer>.Filter.Eq(u => u.Email, email);

    var existing = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    return existing == null;
  }

  private async Task<bool> IsFirstnameLastnameDateofbirthUnique(Guid id, string firstName, string lastName, DateOnly dateOfBirth)
  {
    var filter =
        Builders<Customer>.Filter.Ne(u => u.Id, id) &
        Builders<Customer>.Filter.Eq(u => u.FirstName, firstName) &
        Builders<Customer>.Filter.Eq(u => u.LastName, lastName) &
        Builders<Customer>.Filter.Eq(u => u.DateOfBirth, dateOfBirth);

    var existing = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    return existing == null;
  }
}
