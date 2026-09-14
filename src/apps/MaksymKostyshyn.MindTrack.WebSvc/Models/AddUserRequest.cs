namespace MaksymKostyshyn.MindTrack.WebSvc.Models;

public class AddUserRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public Guid? TherapistId { get; set; }
    public bool IsVulnerable { get; set; }
    public int AIResponseStyleTypeId { get; set; }
}
