using MBS_AUTHORIZATION.Contract.Abstractions.Messages;
using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MBS_AUTHORIZATION.Contract.Services.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;

namespace MBS_AUTHORIZATION.Application.UserCases.Queries.Identitiy;

public class GetLoginGoogleTest(IHttpContextAccessor httpContext) : IQueryHandler<Query.LoginGoolgeTest, string>
{
    public async Task<Result<string>> Handle(Query.LoginGoolgeTest request, CancellationToken cancellationToken)
    {
        await httpContext.HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme);
        //take the Location header from the response
        var redirectUrl = httpContext.HttpContext.Response.Headers.Location.ToString();
        // take the google token
        var token = httpContext.HttpContext.Request.Cookies["Google_Token"];
        return Result.Success(token);
    }
}
