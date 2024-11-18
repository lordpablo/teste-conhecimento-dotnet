using FluentValidation;
using SampleTest.Application.Shared;
using SampleTest.Domain.Enums;
using SampleTest.Resources.Utils;

namespace SampleTest.Application.Features.Client.Commands
{
    public record CreateClientCommand(
    string Name,
    DateTime BirthDate,
    string CPF,
    string Email,
    GenderEnum Gender,
    double MonthRemuneration
    ) : ICommandQuery<string>;

    public class CreateClientValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientValidator()
        {
            RuleFor(c => c.Name)
               .NotEmpty()
               .WithMessage("Name can't be empty.");

            RuleFor(c => c.Name)
              .MaximumLength(200)
              .WithMessage("Name can't have more then 200 length.");

            RuleFor(c => c.BirthDate)
               .NotEmpty().NotNull()
               .WithMessage("BirthDate can't be empty or null.");

            RuleFor(c => c.Email)
               .NotEmpty()
               .WithMessage("Email can't be empty.");

            RuleFor(c => c.Email)
              .MaximumLength(200)
              .WithMessage("Name can't have more then 200 length.");

            RuleFor(c => c.Email)
              .EmailAddress()
              .WithMessage("Email has to be a valid e-mail.");

            RuleFor(c => c.CPF)
               .NotEmpty()
               .WithMessage("CPF can't be empty.");

            RuleFor(c => c.CPF)
               .Must(x => FunctionsUtil.IsCpf(x))
               .WithMessage("CPF is invalid.");

            RuleFor(c => c.BirthDate)
              .Must(x => FunctionsUtil.IsOlderThan18(x))
              .WithMessage("Client must have age 18+.");

            RuleFor(c => c.CPF)
            .MaximumLength(12)
            .WithMessage("CPF can't have more then 11 length.");

            RuleFor(c => c.MonthRemuneration)
            .GreaterThan(-1)
            .WithMessage("MonthRemuneration can't be less then 0.");
        }
    }
}
