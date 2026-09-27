using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Application.Abstractions.Services;

public interface IGoogleOAuthService
{
    public Task<GoogleIdentity> ExchangeAsync(string code, string redirectUri, string? codeVerifier, CancellationToken cancellationToken);
}
