using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.EF.Mappers;

public static class AssignedPracticeMapper
{
    public static AssignedPractices ToEntity(this AssignedPractice assignedPractice)
    {
        return new AssignedPractices
        {
            Id = assignedPractice.Id,
            JournalEntryId = assignedPractice.JournalEntryId,
            PracticeId = assignedPractice.PracticeId,
            PracticeStatusId = (int)assignedPractice.PracticeStatus,
            StartedAt = assignedPractice.StartedAt,
            CompletedAt = assignedPractice.CompletedAt
        };
    }

    public static AssignedPractice ToDomain(this AssignedPractices entity)
    {
        return new AssignedPractice(
            entity.Id,
            entity.JournalEntryId,
            entity.PracticeId,
            entity.PracticeStatusId,
            entity.StartedAt,
            entity.CompletedAt,
            entity.Practice?.ToDomain());
    }
}
