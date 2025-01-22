using System;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using MediatR;

namespace CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Create;

public record CreateCustomerCommand(Customer NewModel) : IRequest<Customer>;
