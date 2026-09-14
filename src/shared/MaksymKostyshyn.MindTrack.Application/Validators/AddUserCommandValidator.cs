using FluentValidation;
using MaksymKostyshyn.MindTrack.Application.Commands;
using MaksymKostyshyn.MindTrack.Domain.Enums;

namespace MaksymKostyshyn.MindTrack.Application.Validators;

public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);

        RuleFor(command => command.FullName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.RoleId)
            .Must(roleId => Enum.IsDefined(typeof(UserRole), roleId))
            .WithMessage("RoleId must be a valid role.");

        RuleFor(command => command.AIResponseStyleTypeId)
            .Must(styleId => Enum.IsDefined(typeof(AIResponseStyleType), styleId))
            .WithMessage("AIResponseStyleTypeId must be a valid AI response style.");
    }
}
