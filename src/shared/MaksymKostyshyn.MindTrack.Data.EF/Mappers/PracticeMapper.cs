using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.EF.Mappers;

public static class PracticeMapper
{
    public static PracticeEntity ToEntity(this Practice practice)
    {
        return new PracticeEntity
        {
            Id = practice.Id,
            Name = practice.Name,
            PracticeTypeId = (int)practice.PracticeType,
            DifficultyLevel = practice.DifficultyLevel,
            UnlockPointsRequired = practice.UnlockPointsRequired,
            IsActive = practice.IsActive
        };
    }

    public static Practice ToDomain(this PracticeEntity entity)
    {
        return new Practice(
            entity.Id,
            entity.Name,
            entity.PracticeTypeId,
            entity.DifficultyLevel,
            entity.UnlockPointsRequired,
            entity.IsActive);
    }
}
