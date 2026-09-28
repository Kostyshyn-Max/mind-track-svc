using System.Text.Json;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Caching;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace MaksymKostyshyn.MindTrack.Data.Redis;

public class RedisCacheService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisCacheService> logger) : ICacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
    private readonly ILogger<RedisCacheService> _logger = logger;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        try
        {
            var database = _connectionMultiplexer.GetDatabase();
            var value = await database.StringGetAsync(key);

            if (!value.HasValue)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>((string)value!);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to read cache key {Key} from Redis.", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken)
    {
        try
        {
            var database = _connectionMultiplexer.GetDatabase();
            var serialized = JsonSerializer.Serialize(value);

            await database.StringSetAsync(key, serialized, expiration);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to write cache key {Key} to Redis.", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        try
        {
            var database = _connectionMultiplexer.GetDatabase();
            await database.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to remove cache key {Key} from Redis.", key);
        }
    }
}
