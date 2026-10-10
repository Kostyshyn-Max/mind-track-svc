using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;

public interface IAchievementQuery
{
    public Task<UserAchievementReport?> GetUnlockedAchievementsForUserAsync(Guid userId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken);

    /// <param name="excludeUnlockedForUserId">When set, achievements this user has already unlocked are left out (and not counted).</param>
    public Task<PagedResult<Achievement>?> GetAllAchievementsAsync(int page, int pageSize, Guid? excludeUnlockedForUserId, CancellationToken cancellationToken);
}
