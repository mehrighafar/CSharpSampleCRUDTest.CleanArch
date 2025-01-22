using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using MediatR;

namespace CSharpSampleCRUDTest.CleanArch.UseCases.Customers.List;

public class GetCustomerListQuery() : IRequest<IEnumerable<Customer>>;
