using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Application.Abstractions.Services;

public interface IExchangeTokenService
{
    public Task<AppAccessToken> ExchangeAsync(string code, string redirectUri, string? codeVerifier, CancellationToken cancellationToken);
}
