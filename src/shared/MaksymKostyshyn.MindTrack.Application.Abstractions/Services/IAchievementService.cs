using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Application.Abstractions.Services;

public interface IAchievementService
{
    public Task<Achievement?> CreateAchievementAsync(string name, string description, int pointsReward, CancellationToken cancellationToken);
    public Task<Achievement?> GetAchievementByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<PagedResult<Achievement>?> GetAllAchievementsAsync(int page, int pageSize, Guid? excludeUnlockedForUserId, CancellationToken cancellationToken);
    public Task<IList<UnlockedAchievement>> GetUnlockedAchievementsAsync(Guid userId, CancellationToken cancellationToken);
    public Task<bool> UpdateAchievementAsync(Guid id, string name, string description, int pointsReward, CancellationToken cancellationToken);
    public Task<bool> DeleteAchievementAsync(Guid id, CancellationToken cancellationToken);
}
