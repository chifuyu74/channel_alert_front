using Microsoft.Extensions.Caching.Distributed;

namespace channel_alert_front.ApiService.Services;

public class TokenBlackListService
{
    private readonly IDistributedCache _cache;
    public const string blacklisted = "blacklisted";

    public TokenBlackListService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task BlackListTokenAsync(string token, TimeSpan expirationTime)
    {
        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = expirationTime,
        };
        await _cache.SetStringAsync(token, blacklisted, options);
    }

    public async Task<bool> IsTokenBlackListedAsync(string token)
    {
        string? value = await _cache.GetStringAsync(token);
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return value == blacklisted;
    }
}
