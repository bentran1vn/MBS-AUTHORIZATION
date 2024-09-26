using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MediatR;

namespace MBS_AUTHORIZATION.Contract.Abstractions.Messages;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
