using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Achievements;

public class GetAchievementUseCase(IAchievementService achievementService)
{
    private readonly IAchievementService _achievementService = achievementService;

    public async Task<Achievement?> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _achievementService.GetAchievementByIdAsync(id, cancellationToken);
        return result;
    }
}
