using MBS_AUTHORIZATION.Contract.Abstractions.Messages;

namespace MBS_AUTHORIZATION.Contract.Services.Identity;

public static class Query
{
    public record Login(string GoogleToken) : IQuery<string>;
    public record Logout :IQuery<string>;
}
