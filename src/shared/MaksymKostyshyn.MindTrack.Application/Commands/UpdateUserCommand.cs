namespace MaksymKostyshyn.MindTrack.Application.Commands;

public record UpdateUserCommand(Guid Id, string Email, string Password, string FullName, int RoleId, Guid? TherapistId, bool IsVulnerable, int AIResponseStyleTypeId);
