using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Auth.Services;

public class GoogleOAuthService : IGoogleOAuthService
{
    public Task<GoogleIdentity> ExchangeAsync(string code, string redirectUri, string? codeVerifier, CancellationToken cancellationToken)
    {

    }
}
