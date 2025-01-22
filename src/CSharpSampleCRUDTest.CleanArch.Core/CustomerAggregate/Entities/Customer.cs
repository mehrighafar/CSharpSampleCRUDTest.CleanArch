using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;

public class Customer
{
  [Required]
  public Guid Id { get; set; }

  [Required]
  public string FirstName { get; set; } = string.Empty;

  [Required]
  public string LastName { get; set; } = string.Empty;

  [Required]
  public DateOnly DateOfBirth { get; set; } = new DateOnly();

  [Required]
  public string PhoneNumber { get; set; } = string.Empty;

  [Required]
  [DataType(DataType.EmailAddress)]
  [EmailAddress]
  public string Email { get; set; } = string.Empty;

  [Required]
  public string BankAccountNumber { get; set; } = string.Empty;
}
