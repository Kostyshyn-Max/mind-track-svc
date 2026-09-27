using System.Net.Http.Json;
using Google.Apis.Auth;
using MaksymKostyshyn.MindTrack.Application.Abstractions.Exceptions;
using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Auth.Models;
using MaksymKostyshyn.MindTrack.Auth.Options;
using MaksymKostyshyn.MindTrack.Domain.Models;
using Microsoft.Extensions.Options;

namespace MaksymKostyshyn.MindTrack.Auth.Services;

public class GoogleOAuthService(IHttpClientFactory httpClientFactory, IOptions<GoogleOAuthOptions> options) : IGoogleOAuthService
{
    private const string TokenEndpoint = "https://oauth2.googleapis.com/token";

    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly GoogleOAuthOptions _options = options.Value;

    public async Task<GoogleIdentity> ExchangeAsync(string code, string redirectUri, string? codeVerifier, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(GoogleOAuthService));

        var parameters = new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = _options.ClientId,
            ["client_secret"] = _options.ClientSecret,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code"
        };

        if (!string.IsNullOrEmpty(codeVerifier))
        {
            parameters["code_verifier"] = codeVerifier;
        }

        using var response = await httpClient.PostAsync(TokenEndpoint, new FormUrlEncodedContent(parameters), cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new GoogleTokenExchangeException($"Google token exchange failed: {response.StatusCode} {errorBody}");
        }

        var tokenResponse = await response.Content.ReadFromJsonAsync<GoogleTokenResponse>(cancellationToken)
            ?? throw new GoogleTokenExchangeException("Google token exchange returned an empty response.");

        if (string.IsNullOrEmpty(tokenResponse.IdToken))
        {
            throw new GoogleTokenExchangeException("Google token exchange response did not contain an id_token.");
        }

        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_options.ClientId]
            });
        }
        catch (InvalidJwtException ex)
        {
            throw new GoogleTokenExchangeException($"Google id_token failed validation: {ex.Message}");
        }

        return new GoogleIdentity(payload.Email, payload.EmailVerified, payload.Subject);
    }
}
