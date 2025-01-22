using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using MediatR;

namespace CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Get;

public record GetCustomerQuery(Guid Id) : IRequest<Customer>;
