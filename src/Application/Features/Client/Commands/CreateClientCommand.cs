using SampleTest.Application.Shared;
using SampleTest.Domain.Enums;

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
}
