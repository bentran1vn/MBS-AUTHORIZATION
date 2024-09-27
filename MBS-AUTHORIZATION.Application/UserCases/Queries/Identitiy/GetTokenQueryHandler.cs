using MBS_AUTHORIZATION.Application.Abstractions;
using MBS_AUTHORIZATION.Contract.Abstractions.Messages;
using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MBS_AUTHORIZATION.Contract.Services.Identity;

namespace MBS_AUTHORIZATION.Application.UserCases.Queries.Identitiy;

public class GetTokenQueryHandler : IQueryHandler<Query.Token, Response.Authenticated>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;
    
    public GetTokenQueryHandler(IJwtTokenService jwtTokenService, ICacheService cacheService)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
    }
    
    public async Task<Result<Response.Authenticated>> Handle(Query.Token request, CancellationToken cancellationToken)
    {
        var (claimPrincipal, isExpired)  = _jwtTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var userAccount = claimPrincipal.Identity!.Name;
        var cacheData = await _cacheService.GetAsync<Response.Authenticated>($"{nameof(Query.Login)}-UserAccount:{userAccount}", cancellationToken);
        
        if (cacheData == null || !cacheData.RefreshToken!.Equals(request.RefreshToken))
        {
            throw new Exception("Invalid refresh token");
        }
        
        if (!isExpired)
        {
            return Result.Success(cacheData);
        }
        
        var accessToken = _jwtTokenService.GenerateAccessToken(claimPrincipal.Claims);
        var response = new Response.Authenticated()
        {
            AccessToken = accessToken,
            RefreshToken = cacheData.RefreshToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5)
        };
        
        await _cacheService.SetAsync($"{nameof(Query.Login)}-UserAccount:{userAccount}", response, null,cancellationToken);

        return Result.Success(response);
    }
}