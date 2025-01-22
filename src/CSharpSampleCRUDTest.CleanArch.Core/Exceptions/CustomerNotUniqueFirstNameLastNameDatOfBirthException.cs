namespace CSharpSampleCRUDTest.CleanArch.Core.Exceptions;

public sealed class CustomerNotUniqueFirstNameLastNameDatOfBirthException : BadRequestException
{
  public CustomerNotUniqueFirstNameLastNameDatOfBirthException
    (string firstName, string lastName, DateOnly dateOfBirth)
       : base($"The customer with" +
         $" first name: {firstName}" +
         $", last name: {lastName}" +
         $", date of birth: {dateOfBirth}  already exists.")
  {
  }
}
