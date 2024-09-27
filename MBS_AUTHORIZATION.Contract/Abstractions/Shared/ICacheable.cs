namespace MBS_AUTHORIZATION.Contract.Abstractions.Shared;

public interface ICacheable
{
    bool BypassCache { get; }
    string CacheKey { get; }
    int SlidingExpirationInMinutes { get; }
    int AbsoluteExpirationInMinutes { get; }
}