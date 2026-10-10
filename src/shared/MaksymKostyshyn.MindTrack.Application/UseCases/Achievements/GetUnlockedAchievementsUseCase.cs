using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Achievements;

public class GetUnlockedAchievementsUseCase(IAchievementService achievementService)
{
    private readonly IAchievementService _achievementService = achievementService;

    public async Task<IList<UnlockedAchievement>> HandleAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _achievementService.GetUnlockedAchievementsAsync(userId, cancellationToken);
    }
}
