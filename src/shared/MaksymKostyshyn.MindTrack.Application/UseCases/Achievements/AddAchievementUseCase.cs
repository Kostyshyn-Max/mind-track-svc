using FluentValidation;
using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Application.Commands;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Achievements;

public class AddAchievementUseCase(IAchievementService achievementService, IValidator<AddAchievementCommand> validator)
{
    private readonly IAchievementService _achievementService = achievementService;
    private readonly IValidator<AddAchievementCommand> _validator = validator;

    public async Task<Achievement?> HandleAsync(AddAchievementCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var result = await _achievementService.CreateAchievementAsync(command.Name, command.Description, command.PointsReward, cancellationToken);
        return result;
    }
}
