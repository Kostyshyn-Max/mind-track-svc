using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.EF.Mappers;

public static class AchievementMapper
{
    public static AchievementEntity ToEntity(this Achievement achievement)
    {
        return new AchievementEntity
        {
            Id = achievement.Id,
            Name = achievement.Name,
            Description = achievement.Description,
            PointsReward = achievement.PointsReward
        };
    }

    public static Achievement ToDomain(this AchievementEntity entity)
    {
        return new Achievement(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.PointsReward);
    }
}
