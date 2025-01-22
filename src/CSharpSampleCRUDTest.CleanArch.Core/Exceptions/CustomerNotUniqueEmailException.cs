namespace CSharpSampleCRUDTest.CleanArch.Core.Exceptions;

public sealed class CustomerNotUniqueEmailException : BadRequestException
{
  public CustomerNotUniqueEmailException(string email)
       : base($"The customer with the email {email} already exists.")
  {
  }
}
