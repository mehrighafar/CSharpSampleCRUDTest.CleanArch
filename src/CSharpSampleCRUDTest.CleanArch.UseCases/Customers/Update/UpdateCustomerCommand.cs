using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using MediatR;

namespace CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Update;

public record UpdateCustomerCommand(Customer NewModel) : IRequest<Customer>;
