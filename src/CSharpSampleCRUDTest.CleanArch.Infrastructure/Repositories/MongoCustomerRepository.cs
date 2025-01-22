using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.Core.Exceptions;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SharpCompress.Common;

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
    _logger.BeginScope("Creating customer: {model} in repository.", entity);

    entity.Id = Guid.NewGuid();
    await _mongoCollection.InsertOneAsync(entity);
    return entity;
  }

  public async Task<Customer?> UpdateAsync(Customer entity)
  {
    _logger.BeginScope("Updating customer: {model} in repository.", entity);

    return await _mongoCollection.FindOneAndReplaceAsync(e => e.Id == entity.Id, entity);
  }

  public async Task<DeleteResult> RemoveAsync(Guid id)
  {
    _logger.BeginScope("Removing customer from repository with id: {id}.", id);

    return await _mongoCollection.DeleteOneAsync(e => e.Id == id);
  }
}
