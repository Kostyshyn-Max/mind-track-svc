using System.ComponentModel.DataAnnotations;

namespace MaksymKostyshyn.MindTrack.Auth.Options;

public class JwtOptions
{
    public const string SectionName = "Authentication:Jwt";

    [Required]
    public required string Issuer { get; set; }

    [Required]
    public required string Audience { get; set; }

    [Required]
    [MinLength(32)]
    public required string SigningKey { get; set; }

    [Range(1, int.MaxValue)]
    public int ExpiresInMinutes { get; set; } = 60;
}
