using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Application.Services;

public class AchievementService(IAchievementRepository achievementRepository, IAchievementQuery achievementQuery) : IAchievementService
{
    private readonly IAchievementRepository _achievementRepository = achievementRepository;
    private readonly IAchievementQuery _achievementQuery = achievementQuery;

    public async Task<Achievement?> CreateAchievementAsync(string name, string description, int pointsReward, CancellationToken cancellationToken)
    {
        var achievement = new Achievement(Guid.NewGuid(), name, description, pointsReward);

        var result = await _achievementRepository.CreateAchievementAsync(achievement, cancellationToken);
        return result;
    }

    public async Task<Achievement?> GetAchievementByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _achievementRepository.GetAchievementByIdAsync(id, cancellationToken);
        return result;
    }

    public async Task<PagedResult<Achievement>?> GetAllAchievementsAsync(int page, int pageSize, Guid? excludeUnlockedForUserId, CancellationToken cancellationToken)
    {
        var result = await _achievementQuery.GetAllAchievementsAsync(page, pageSize, excludeUnlockedForUserId, cancellationToken);
        return result;
    }

    public async Task<IList<UnlockedAchievement>> GetUnlockedAchievementsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var report = await _achievementQuery.GetUnlockedAchievementsForUserAsync(userId, null, null, cancellationToken);

        // The query yields no report when the user has nothing unlocked.
        return report?.UnlockedAchievements ?? [];
    }

    public async Task<bool> UpdateAchievementAsync(Guid id, string name, string description, int pointsReward, CancellationToken cancellationToken)
    {
        var achievement = new Achievement(id, name, description, pointsReward);

        var result = await _achievementRepository.UpdateAchievementAsync(achievement, cancellationToken);
        return result;
    }

    public async Task<bool> DeleteAchievementAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _achievementRepository.DeleteAchievementAsync(id, cancellationToken);
        return result;
    }
}
