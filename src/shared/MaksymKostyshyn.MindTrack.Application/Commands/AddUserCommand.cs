namespace MaksymKostyshyn.MindTrack.Application.Commands;

public record AddUserCommand(string Email, string Password, string FullName, int RoleId, Guid? TherapistId, bool IsVulnerable, int AIResponseStyleTypeId);
