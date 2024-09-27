using MBS_AUTHORIZATION.Application.Abstractions;
using MBS_AUTHORIZATION.Contract.Abstractions.Messages;
using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MBS_AUTHORIZATION.Contract.Services.Identity;

namespace MBS_AUTHORIZATION.Application.UserCases.Commands.Identity;

public class LogoutCommandHandler : ICommandHandler<Command.LogoutCommand>
{
    private readonly ICacheService _cacheService;

    public LogoutCommandHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(Command.LogoutCommand request, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync($"{nameof(Query.Login)}-UserAccount:{request.UserAccount}", cancellationToken);
        return Result.Success("Logout Successfully");
    }
}