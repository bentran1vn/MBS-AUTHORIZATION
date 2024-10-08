﻿using System.Text;
using MBS_AUTHORIZATION.Contract.Abstractions.Messages;
using MBS_AUTHORIZATION.Contract.Abstractions.Shared;

namespace MBS_AUTHORIZATION.Contract.Services.Identity;

public static class Query
{
    public record LoginGoogle(string GoogleToken) : IQuery<Response.Authenticated>;

    public record LoginGoolgeTest : IQuery<string>;
    public record LogoutGoogle :IQuery<string>;
    
    public record Login(string EmailOrUserName, string Password) : IQuery<Response.Authenticated>, ICacheable
    {
        public bool BypassCache => true;
        public string CacheKey {
            get
            {
                var builder = new StringBuilder();
                builder.Append($"{nameof(Login)}");
                builder.Append($"-UserAccount:{EmailOrUserName}");
                return builder.ToString();
            }
        }
        public int SlidingExpirationInMinutes => 10;
        public int AbsoluteExpirationInMinutes => 15;
    }

    public record Token(string AccessToken, string RefreshToken) : IQuery<Response.Authenticated>;
}
