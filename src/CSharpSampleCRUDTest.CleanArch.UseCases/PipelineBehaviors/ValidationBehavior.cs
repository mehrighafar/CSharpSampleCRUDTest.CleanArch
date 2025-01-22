using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationException = CSharpSampleCRUDTest.CleanArch.UseCases.Exceptions.ValidationException;

namespace CSharpSampleCRUDTest.CleanArch.UseCases.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
  private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    if (!_validators.Any())
    {
      return await next();
    }

    var context = new ValidationContext<TRequest>(request);

    var failures = _validators
        .Select(x => x.Validate(context))
        .SelectMany(x => x.Errors)
        .Where(x => x is not null)
        .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                  Key = propertyName,
                  Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Values);


    if (failures.Count != 0)
    {
      throw new ValidationException(failures);
    }

    return await next();
  }
}
