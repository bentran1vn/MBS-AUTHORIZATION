﻿using System.Security.Claims;
using Carter;
using MBS_AUTHORIZATION.Application.Abstractions;
using MBS_AUTHORIZATION.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MBS_AUTHORIZATION.Presentation.APIs.Identity;

using QueryV1 = Contract.Services.Identity.Query;
using CommandV1 = Contract.Services.Identity.Command;

public class AuthApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/auth";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Authentication")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost("login_google", LoginGoogleV1);
        group1.MapGet("logout_google", LogoutGoogleV1).RequireAuthorization();
        group1.MapPost("login", LoginV1);
        group1.MapPost("register", RegisterV1);
        group1.MapPost("refresh_token", RefreshTokenV1);
        group1.MapPost("forgot_password", ForgotPasswordV1);
        group1.MapPost("verify_code", VerifyCodeV1);
        group1.MapPost("change_password", ChangePasswordV1).RequireAuthorization();
        group1.MapGet("logout", LogoutV1).RequireAuthorization();
    }

    public static async Task<IResult> LoginGoogleV1(ISender sender, QueryV1.LoginGoogle login)
    {
        var result = await sender.Send(login);

        if (result.IsFailure)
            return HandlerFailure(result);
        return Results.Ok(result);
    }
    public static async Task<IResult> LogoutGoogleV1(ISender sender)
    {
        var result = await sender.Send(new QueryV1.LogoutGoogle());

        if (result.IsFailure)
            return HandlerFailure(result);
        return Results.Ok(result);
    }
    
    public static async Task<IResult> LoginV1(ISender sender, [FromBody] QueryV1.Login login)
    {
        var result = await sender.Send(login);
        
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
    public static async Task<IResult> RefreshTokenV1(HttpContext context, ISender sender, [FromBody] QueryV1.Token query)
    {
        //var accessToken = await context.GetTokenAsync("access_token");
        var result = await sender.Send(query);
        
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
    public static async Task<IResult> LogoutV1(ISender sender, HttpContext context, IJwtTokenService jwtTokenService)
    {
        var accessToken = await context.GetTokenAsync("access_token");
        var (claimPrincipal, _)  = jwtTokenService.GetPrincipalFromExpiredToken(accessToken!);
        var email = claimPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
        
        var result = await sender.Send(new CommandV1.LogoutCommand(email));
        
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
    
    public static async Task<IResult> ForgotPasswordV1(ISender sender, [FromBody] CommandV1.ForgotPasswordCommand command)
    {
        var result = await sender.Send(command);
        
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
    
    public static async Task<IResult> VerifyCodeV1(ISender sender, [FromBody] CommandV1.VerifyCodeCommand command)
    {
        var result = await sender.Send(command);
        
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
    
    public static async Task<IResult> ChangePasswordV1(ISender sender, HttpContext context, IJwtTokenService jwtTokenService, [FromBody] CommandV1.ChangePasswordCommand command)
    {
        var accessToken = await context.GetTokenAsync("access_token");
        var (claimPrincipal, _)  = jwtTokenService.GetPrincipalFromExpiredToken(accessToken!);
        var email = claimPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
        var result = await sender.Send(new CommandV1.ChangePasswordCommand(email, command.NewPassword));
        
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
    
    public static async Task<IResult> RegisterV1(ISender sender, [FromBody] CommandV1.RegisterCommand command)
    {
        var result = await sender.Send(command);
        
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
    
}