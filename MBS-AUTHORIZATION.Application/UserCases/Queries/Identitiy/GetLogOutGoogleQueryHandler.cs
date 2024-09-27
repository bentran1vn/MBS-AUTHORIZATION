using MBS_AUTHORIZATION.Contract.Abstractions.Messages;
using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MBS_AUTHORIZATION.Contract.Services.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace MBS_AUTHORIZATION.Application.UserCases.Queries.Identitiy;

public class GetLogoutGoogleQueryHandler(IHttpContextAccessor _httpContext) : IQueryHandler<Query.LogoutGoogle, string>
{
    public async Task<Result<string>> Handle(Query.LogoutGoogle request, CancellationToken cancellationToken)
    {
        await _httpContext.HttpContext.SignOutAsync();
        return Result.Success("Logged out");
    }
}
