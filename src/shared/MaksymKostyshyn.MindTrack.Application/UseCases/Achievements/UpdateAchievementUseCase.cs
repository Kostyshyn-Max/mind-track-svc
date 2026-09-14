using FluentValidation;
using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Application.Commands;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Achievements;

public class UpdateAchievementUseCase(IAchievementService achievementService, IValidator<UpdateAchievementCommand> validator)
{
    private readonly IAchievementService _achievementService = achievementService;
    private readonly IValidator<UpdateAchievementCommand> _validator = validator;

    public async Task<bool> HandleAsync(UpdateAchievementCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var result = await _achievementService.UpdateAchievementAsync(command.Id, command.Name, command.Description, command.PointsReward, cancellationToken);
        return result;
    }
}
