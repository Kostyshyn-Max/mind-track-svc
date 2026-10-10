using MaksymKostyshyn.MindTrack.Data.Abstractions.Caching;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Data.Redis.Queries;

public class AchievementQueryCacheDecorator(IAchievementQuery innerQuery, ICacheService cacheService) : IAchievementQuery
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);

    private readonly IAchievementQuery _innerQuery = innerQuery;
    private readonly ICacheService _cacheService = cacheService;

    public Task<UserAchievementReport?> GetUnlockedAchievementsForUserAsync(Guid userId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken)
    {
        return _innerQuery.GetUnlockedAchievementsForUserAsync(userId, fromDate, toDate, cancellationToken);
    }

    public async Task<PagedResult<Achievement>?> GetAllAchievementsAsync(int page, int pageSize, Guid? excludeUnlockedForUserId, CancellationToken cancellationToken)
    {
        // Per-user results change as soon as the user unlocks something, so only the shared catalogue is cached.
        if (excludeUnlockedForUserId is not null)
        {
            return await _innerQuery.GetAllAchievementsAsync(page, pageSize, excludeUnlockedForUserId, cancellationToken);
        }

        var cacheKey = $"achievements:v2:page:{page}:pageSize:{pageSize}";

        var cached = await _cacheService.GetAsync<PagedResult<Achievement>>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return cached;
        }

        var result = await _innerQuery.GetAllAchievementsAsync(page, pageSize, null, cancellationToken);

        if (result is not null)
        {
            await _cacheService.SetAsync(cacheKey, result, CacheDuration, cancellationToken);
        }

        return result;
    }
}
