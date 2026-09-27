using MaksymKostyshyn.MindTrack.Application.Abstractions.Exceptions;
using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Application.Services;

public class ExchangeTokenService(IGoogleOAuthService googleOAuthService, IUserQuery userQuery, IAppTokenGeneratorService appTokenGeneratorService) : IExchangeTokenService
{
    private readonly IGoogleOAuthService _googleOAuthService = googleOAuthService;
    private readonly IUserQuery _userQuery = userQuery;
    private readonly IAppTokenGeneratorService _appTokenGeneratorService = appTokenGeneratorService;

    public async Task<AppAccessToken> ExchangeAsync(string code, string redirectUri, string? codeVerifier, CancellationToken cancellationToken)
    {
        var identity = await _googleOAuthService.ExchangeAsync(code, redirectUri, codeVerifier, cancellationToken);

        if (!identity.EmailVerified)
        {
            throw new GoogleTokenExchangeException("Google email not verified.");
        }

        var user = await _userQuery.GetByEmailAsync(identity.Email, cancellationToken) ?? throw new UserNotRegisteredException(identity.Email);
        if (!user.IsActive)
        {
            throw new InactiveUserException(identity.Email);
        }

        var jwt = _appTokenGeneratorService.GenerateToken(user);
        return new AppAccessToken(jwt, "Bearer", 3600);
    }
}
