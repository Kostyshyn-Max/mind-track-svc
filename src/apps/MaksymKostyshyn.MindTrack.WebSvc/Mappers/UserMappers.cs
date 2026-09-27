using MaksymKostyshyn.MindTrack.Application.Commands;
using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.WebSvc.Models.User;

namespace MaksymKostyshyn.MindTrack.WebSvc.Mappers;

public static class UserMappers
{
    public static AddUserCommand ToCommand(this AddUserRequest request)
    {
        return new AddUserCommand(request.Email, request.Password, request.FullName, request.RoleId, request.TherapistId, request.IsVulnerable, request.AIResponseStyleTypeId);
    }

    public static UpdateUserCommand ToCommand(this UpdateUserRequest request, Guid id)
    {
        return new UpdateUserCommand(id, request.Email, request.Password, request.FullName, request.RoleId, request.TherapistId, request.IsVulnerable, request.AIResponseStyleTypeId);
    }

    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            TherapistId = user.TherapistId,
            IsVulnerable = user.IsVulnerable,
            AIResponseStyleType = user.AIResponseStyleType.ToString(),
            EarnedPoints = user.EarnedPoints,
        };
    }
}
