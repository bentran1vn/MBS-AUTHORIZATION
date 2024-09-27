using Carter;
using MBS_AUTHORIZATION.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DistributedSystem.Presentation.APIs.Identity;

public class AuthApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/auth";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Authentication")
            .MapGroup(BaseUrl).HasApiVersion(1).RequireAuthorization();

        group1.MapPost("login", LoginV1).AllowAnonymous();
        group1.MapGet("logout", LogoutV1).RequireAuthorization();
    }

    public static async Task<IResult> LoginV1(ISender sender, MBS_AUTHORIZATION.Contract.Services.Identity.Query.Login login)
    {
        var result = await sender.Send(login);

        if (result.IsFailure)
            return HandlerFailure(result);
        return Results.Ok(result);
    }
    public static async Task<IResult> LogoutV1(ISender sender)
    {
        var result = await sender.Send(new MBS_AUTHORIZATION.Contract.Services.Identity.Query.Logout());

        if (result.IsFailure)
            return HandlerFailure(result);
        return Results.Ok(result);
    }
}