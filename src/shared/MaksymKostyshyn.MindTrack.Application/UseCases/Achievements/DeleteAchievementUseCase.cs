using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Achievements;

public class DeleteAchievementUseCase(IAchievementService achievementService)
{
    private readonly IAchievementService _achievementService = achievementService;

    public async Task<bool> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _achievementService.DeleteAchievementAsync(id, cancellationToken);
        return result;
    }
}
