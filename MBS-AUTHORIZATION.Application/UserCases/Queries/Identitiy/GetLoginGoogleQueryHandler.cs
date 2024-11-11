using Google.Apis.Auth;
using MBS_AUTHORIZATION.Application.Abstractions;
using MBS_AUTHORIZATION.Contract.Abstractions.Messages;
using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MBS_AUTHORIZATION.Contract.Services.Identity;
using MBS_AUTHORIZATION.Domain.Abstractions;
using MBS_AUTHORIZATION.Domain.Abstractions.Repositories;
using MBS_AUTHORIZATION.Domain.Entities;
using System.Security.Claims;
using MBS_AUTHORIZATION.Persistence;
using Newtonsoft.Json;


namespace MBS_AUTHORIZATION.Application.UserCases.Queries.Identitiy;

public class GetLoginGoogleQueryHandler(IRepositoryBase<User, Guid> repositoryBase, IUnitOfWork eFUnitOfWork, IJwtTokenService jwtTokenService, IPasswordHasherService passwordHasherService, ApplicationDbContext context) : IQueryHandler<Query.LoginGoogle, Response.Authenticated>
{
    public async Task<Result<Response.Authenticated>> Handle(Query.LoginGoogle request, CancellationToken cancellationToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleToken);
        if (payload == null)
        {
            return (Result<Response.Authenticated>)Result.Failure(new Error("404", "Invalid Google Token"));
        }
        var user = await repositoryBase.FindSingleAsync(x => x.Email == payload.Email, cancellationToken);
        int status = 1;
        int role = 0;
        var hashedPassword = passwordHasherService.HashPassword("12345");

        if (user == null)
        {
            /*List<string> emails =
            [
                "nghi",
                "tantdtse171757@fpt.edu.vn",
                "lam",
                "son",
            ];*/
            var em = context.Configs.Where(x => x.Key.Equals("ListOfAdminAccountContain")).Select(x => x.Value).FirstOrDefault();
            var emails = JsonConvert.DeserializeObject<List<string>>(em);
           
            if (emails.Exists(payload.Email.Contains))
            {
                status = 0;
                role = 1;
            }
            else if(payload.Email.Contains("tan182205@gmail.com"))
            {
                role=0;
            }
            user = new User
            {
                Email = payload.Email,
                FullName = payload.Name,
                Points = 0,
                Role = role,
                Status = status,
                Password = hashedPassword,
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
