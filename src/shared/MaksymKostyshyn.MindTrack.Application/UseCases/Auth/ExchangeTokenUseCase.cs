using FluentValidation;
using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Application.Commands;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Auth;

public class ExchangeTokenUseCase(IExchangeTokenService exchangeTokenService, IValidator<ExchangeTokenCommand> validator)
{
    private readonly IExchangeTokenService _exchangeTokenService = exchangeTokenService;
    private readonly IValidator<ExchangeTokenCommand> _validator = validator;

    public async Task<AppAccessToken> ExecuteAsync(ExchangeTokenCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var result = await _exchangeTokenService.ExchangeAsync(command.Code, command.RedirectUri, command.CodeVerifier, cancellationToken);
        return result;
    }
}
