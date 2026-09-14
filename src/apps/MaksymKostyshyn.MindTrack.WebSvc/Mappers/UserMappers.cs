using MaksymKostyshyn.MindTrack.Application.Commands;
using MaksymKostyshyn.MindTrack.WebSvc.Models;

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
}
