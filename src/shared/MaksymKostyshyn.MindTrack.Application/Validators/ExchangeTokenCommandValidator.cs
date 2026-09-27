using FluentValidation;
using MaksymKostyshyn.MindTrack.Application.Commands;

namespace MaksymKostyshyn.MindTrack.Application.Validators;

public class ExchangeTokenCommandValidator : AbstractValidator<ExchangeTokenCommand>
{
    public ExchangeTokenCommandValidator()
    {
        RuleFor(command => command.Code)
            .NotEmpty();

        RuleFor(command => command.CodeVerifier)
            .NotEmpty();

        RuleFor(command => command.RedirectUri)
            .NotEmpty();
    }
}
