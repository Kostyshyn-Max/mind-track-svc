namespace MaksymKostyshyn.MindTrack.Application.Commands;

public record ExchangeTokenCommand(string Code, string RedirectUri, string CodeVerifier);