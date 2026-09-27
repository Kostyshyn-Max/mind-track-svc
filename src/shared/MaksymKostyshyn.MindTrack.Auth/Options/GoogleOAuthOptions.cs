using System.ComponentModel.DataAnnotations;

namespace MaksymKostyshyn.MindTrack.Auth.Options;

public class GoogleOAuthOptions
{
    public const string SectionName = "Authentication:Google";

    [Required]
    public required string ClientId { get; set; }

    [Required]
    public required string ClientSecret { get; set; }
}
