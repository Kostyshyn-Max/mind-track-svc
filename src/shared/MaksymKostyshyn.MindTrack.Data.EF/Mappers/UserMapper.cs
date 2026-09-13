using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.EF.Mappers;

public static class UserMapper
{
    public static UserEntity ToEntity(this User user)
    {
        return new UserEntity
        {
            Id = user.Id,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            FullName = user.FullName,
            RoleId = (int)user.Role,
            TherapistId = user.TherapistId,
            IsVulnerable = user.IsVulnerable,
            AIResponseStyleId = (int)user.AIResponseStyleType,
            EarnedPoints = user.EarnedPoints,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public static User ToDomain(this UserEntity user)
    {
        return new User(
            user.Id,
            user.Email,
            user.PasswordHash,
            user.FullName,
            user.RoleId,
            user.TherapistId,
            user.IsVulnerable,
            user.AIResponseStyleId,
            user.EarnedPoints,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt);
    }
}
