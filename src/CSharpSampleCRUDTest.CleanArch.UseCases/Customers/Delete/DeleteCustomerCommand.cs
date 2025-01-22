using System;
using MediatR;

namespace CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Delete;

public record DeleteCustomerCommand(Guid Id) : IRequest<bool>;
