using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Application.Services;

public class AchievementService(IAchievementRepository achievementRepository) : IAchievementService
{
    private readonly IAchievementRepository _achievementRepository = achievementRepository;

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
