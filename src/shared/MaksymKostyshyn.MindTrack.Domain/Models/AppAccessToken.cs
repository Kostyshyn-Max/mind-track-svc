namespace MaksymKostyshyn.MindTrack.Domain.Models;

public record AppAccessToken(string AccessToken, string TokenType, int ExpiresIn);