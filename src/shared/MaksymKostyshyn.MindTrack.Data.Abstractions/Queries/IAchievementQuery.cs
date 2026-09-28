using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;

public interface IAchievementQuery
{
    public Task<UserAchievementReport?> GetUnlockedAchievementsForUserAsync(Guid userId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken);
    public Task<IList<Achievement>?> GetAllAchievementsAsync(int page, int pageSize, CancellationToken cancellationToken);
}
