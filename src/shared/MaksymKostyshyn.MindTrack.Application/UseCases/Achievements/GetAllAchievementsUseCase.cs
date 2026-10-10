using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Achievements;

public class GetAllAchievementsUseCase(IAchievementService achievementService)
{
    private readonly IAchievementService _achievementService = achievementService;

    public async Task<PagedResult<Achievement>?> HandleAsync(int page, int pageSize, Guid? excludeUnlockedForUserId, CancellationToken cancellationToken)
    {
        var result = await _achievementService.GetAllAchievementsAsync(page, pageSize, excludeUnlockedForUserId, cancellationToken);
        return result;
    }
}
