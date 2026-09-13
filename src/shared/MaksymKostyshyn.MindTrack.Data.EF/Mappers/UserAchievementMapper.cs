using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.EF.Mappers;

public static class UserAchievementMapper
{
    public static UserAchievementEntity ToEntity(this UserAchievement userAchievement)
    {
        return new UserAchievementEntity
        {
            Id = userAchievement.Id,
            PatientId = userAchievement.PatientId,
            AchievementId = userAchievement.AchievementId,
            UnlockedAt = userAchievement.UnlockedAt
        };
    }

    public static UserAchievement ToDomain(this UserAchievementEntity entity)
    {
        return new UserAchievement(
            entity.Id,
            entity.PatientId,
            entity.AchievementId,
            entity.UnlockedAt);
    }
}
