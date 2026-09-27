using Microsoft.AspNetCore.Mvc;

namespace MaksymKostyshyn.MindTrack.WebSvc.Models.Auth;

public class TokenExchangeRequest
{
    [FromForm(Name = "grant_type")]
    public string? GrantType { get; set; }

    [FromForm(Name = "code")]
    public string? Code { get; set; }

    [FromForm(Name = "redirect_uri")]
    public string? RedirectUri { get; set; }

    [FromForm(Name = "client_id")]
    public string? ClientId { get; set; }

    [FromForm(Name = "code_verifier")]
    public string? CodeVerifier { get; set; }
}
