namespace CSharpSampleCRUDTest.CleanArch.UseCases.Customers;
public class CustomerDTO
{
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public DateOnly DateOfBirth { get; set; } = new DateOnly();
  public string PhoneNumber { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string BankAccountNumber { get; set; } = string.Empty;
}
