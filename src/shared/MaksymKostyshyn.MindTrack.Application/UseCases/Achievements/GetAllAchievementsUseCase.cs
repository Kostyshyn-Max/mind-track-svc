using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Achievements;

public class GetAllAchievementsUseCase(IAchievementService achievementService)
{
    private readonly IAchievementService _achievementService = achievementService;

    public async Task<IList<Achievement>?> HandleAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _achievementService.GetAllAchievementsAsync(page, pageSize, cancellationToken);
        return result;
    }
}
