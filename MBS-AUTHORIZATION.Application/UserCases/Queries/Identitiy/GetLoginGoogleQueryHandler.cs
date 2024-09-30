using Google.Apis.Auth;
using MBS_AUTHORIZATION.Application.Abstractions;
using MBS_AUTHORIZATION.Contract.Abstractions.Messages;
using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MBS_AUTHORIZATION.Contract.Services.Identity;
using MBS_AUTHORIZATION.Domain.Abstractions;
using MBS_AUTHORIZATION.Domain.Abstractions.Repositories;
using MBS_AUTHORIZATION.Domain.Entities;
using MBS_AUTHORIZATION.Persistence;
using System.Security.Claims;


namespace MBS_AUTHORIZATION.Application.UserCases.Queries.Identitiy;

public class GetLoginGoogleQueryHandler(IRepositoryBase<User, Guid> repositoryBase, IUnitOfWork eFUnitOfWork, IJwtTokenService jwtTokenService) : IQueryHandler<Query.LoginGoogle, Response.Authenticated>
{
    public async Task<Result<Response.Authenticated>> Handle(Query.LoginGoogle request, CancellationToken cancellationToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleToken) ?? throw new Exception("Invalid Google Token");
        var user = await repositoryBase.FindSingleAsync(x => x.Email == payload.Email, cancellationToken);

        if (user == null)
        {
            user = new User
            {
                Email = payload.Email,
                FullName = payload.Name,
                Points = 0,
                Role = 1,
                Status = 1,
                Password = "123456",
            };

            repositoryBase.Add(user);
            await eFUnitOfWork.SaveChangesAsync(cancellationToken);
        }

        var expirationTime = DateTime.Now.AddMinutes(5);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("Role", user.Role.ToString()),
            new("UserId", user.Id.ToString()),
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Expired, expirationTime.ToString())
        };

        var accessToken = jwtTokenService.GenerateAccessToken(claims);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        var response = new Response.Authenticated
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = expirationTime
        };

        return Result.Success(response);
    }
}
