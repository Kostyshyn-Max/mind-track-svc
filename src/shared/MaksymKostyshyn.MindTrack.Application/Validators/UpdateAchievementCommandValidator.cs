using FluentValidation;
using MaksymKostyshyn.MindTrack.Application.Commands;

namespace MaksymKostyshyn.MindTrack.Application.Validators;

public class UpdateAchievementCommandValidator : AbstractValidator<UpdateAchievementCommand>
{
    public UpdateAchievementCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(command => command.PointsReward)
            .GreaterThanOrEqualTo(0);
    }
}
