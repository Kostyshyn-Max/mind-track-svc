namespace MaksymKostyshyn.MindTrack.WebSvc.Models.User;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public Guid? TherapistId { get; set; }
    public bool IsVulnerable { get; set; }
    public string AIResponseStyleType { get; set; } = string.Empty;
    public int EarnedPoints { get; set; }
}
