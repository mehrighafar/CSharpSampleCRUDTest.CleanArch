using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpSampleCRUDTest.CleanArch.Core.Exceptions;

public sealed class CustomerNotFoundException : NotFoundException
{
  public CustomerNotFoundException(Guid id)
      : base($"The customer with the identifier {id} was not found.")
  {
  }
}
