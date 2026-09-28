using MaksymKostyshyn.MindTrack.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace MaksymKostyshyn.MindTrack.Auth.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class RequireRoleAttribute(params UserRole[] roles) : AuthorizeAttribute, IAuthorizationRequirementData
{
    public IReadOnlyCollection<UserRole> AllowedRoles { get; } = roles;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return new RoleRequirement(AllowedRoles);
    }
}
