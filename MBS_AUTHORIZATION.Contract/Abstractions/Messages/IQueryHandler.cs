using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MediatR;

namespace MBS_AUTHORIZATION.Contract.Abstractions.Messages;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
