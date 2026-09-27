using System.Text.Json.Serialization;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.WebSvc.Models.Auth;

public class TokenExchangeResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public required string TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    public static TokenExchangeResponse FromDomain(AppAccessToken token) => new()
    {
        AccessToken = token.AccessToken,
        TokenType = token.TokenType,
        ExpiresIn = token.ExpiresIn
    };
}
