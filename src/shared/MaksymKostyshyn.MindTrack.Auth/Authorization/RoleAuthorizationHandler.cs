using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MaksymKostyshyn.MindTrack.Auth.Authorization;

public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;

        if (roleClaim is not null
            && requirement.AllowedRoles.Any(role => role.ToString().Equals(roleClaim, StringComparison.OrdinalIgnoreCase)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
