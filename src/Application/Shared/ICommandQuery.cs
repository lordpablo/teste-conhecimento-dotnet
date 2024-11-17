using MediatR;
using SampleTest.Resources.Features;

namespace SampleTest.Application.Shared;

public interface ICommandQuery<TResult> : IRequest<Result<TResult>>
{
}

public interface ICommandQuery : IRequest<Result>
{
}
