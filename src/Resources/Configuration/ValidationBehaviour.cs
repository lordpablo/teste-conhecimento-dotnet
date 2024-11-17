using FluentValidation;
using FluentValidation.Results;
using Humanizer;
using MediatR;
using SampleTest.Resources.Exceptions;


namespace SampleTest.Resources.Configuration
{
    public class ValidationBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResult = await _validators.First().ValidateAsync(context, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var failures = Serialize(validationResult.Errors);
                    throw new BadRequestException(Resources.Messages.BadRequest, failures);
                }
            }
            return await next();
        }

        private static Dictionary<string, string[]> Serialize(IEnumerable<ValidationFailure> failures)
        {
            var camelCaseFailures = failures
                .GroupBy(failure => failure.PropertyName.Camelize())
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(failure => failure.ErrorMessage).ToArray()
                );

            return camelCaseFailures;
        }
    }
}
