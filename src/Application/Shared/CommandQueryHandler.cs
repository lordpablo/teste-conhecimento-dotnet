using MediatR;
using SampleTest.Resources.Features;

namespace SampleTest.Application.Shared
{
    public class CommandQueryHandler<TResult> : IRequestHandler<ICommandQuery<TResult>, Result<TResult>>
    {
        public Task<Result<TResult>> Handle(ICommandQuery<TResult> request, CancellationToken cancellationToken)
        {
            // Implemente a lógica de manuseio aqui
            return Task.FromResult(new Result<TResult>());
        }
    }
}
