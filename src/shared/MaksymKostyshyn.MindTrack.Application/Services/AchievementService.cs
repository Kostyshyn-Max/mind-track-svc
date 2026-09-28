using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Domain;

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

    public async Task<IList<Achievement>?> GetAllAchievementsAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _achievementQuery.GetAllAchievementsAsync(page, pageSize, cancellationToken);
        return result;
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
