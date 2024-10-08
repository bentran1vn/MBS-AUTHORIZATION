using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MediatR;

namespace MBS_AUTHORIZATION.Contract.Abstractions.Messages;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
