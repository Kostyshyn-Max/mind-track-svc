using FluentValidation;
using MaksymKostyshyn.MindTrack.Application.Commands;

namespace MaksymKostyshyn.MindTrack.Application.Validators;

public class AddAchievementCommandValidator : AbstractValidator<AddAchievementCommand>
{
    public AddAchievementCommandValidator()
    {
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
