using Google.Apis.Auth;
using MBS_AUTHORIZATION.Application.Abstractions;
using MBS_AUTHORIZATION.Contract.Abstractions.Messages;
using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MBS_AUTHORIZATION.Contract.Services.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;


namespace MBS_AUTHORIZATION.Application.UserCases.Queries.Identitiy;

public class GetLoginQueryHandler(IHttpContextAccessor httpContext) : IQueryHandler<Query.Login, /*Response.Authenticated*/ string>
{
    public async Task<Result<string>> Handle(Query.Login request, CancellationToken cancellationToken)
    {
        //var payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleToken);
        //if (payload == null)
        //    return (Result<string>)Result.Failure(Error.NullValue);
        //if (payload.Audience != "475317717183-atq7ughidhmier07c1udn2gbdre1q9mt.apps.googleusercontent.com")
        //    return (Result<string>)Result.Failure(Error.InvalidAudience);
        //var googleId = payload.Subject;
      //  var email = payload.Email;
      await httpContext.HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme);

        return (Result<string>)Result<string>.Success("123");
    }
}
