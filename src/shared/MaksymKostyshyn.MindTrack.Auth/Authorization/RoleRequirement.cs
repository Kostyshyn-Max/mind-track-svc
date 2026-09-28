using MaksymKostyshyn.MindTrack.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace MaksymKostyshyn.MindTrack.Auth.Authorization;

public class RoleRequirement(params IReadOnlyCollection<UserRole> allowedRoles) : IAuthorizationRequirement
{
    public IReadOnlyCollection<UserRole> AllowedRoles { get; } = allowedRoles;
}
