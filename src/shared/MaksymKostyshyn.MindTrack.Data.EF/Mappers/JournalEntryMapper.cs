using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.EF.Mappers;

public static class JournalEntryMapper
{
    public static JournalEntryEntity ToEntity(this JournalEntry journalEntry)
    {
        return new JournalEntryEntity
        {
            Id = journalEntry.Id,
            PatientId = journalEntry.PatientId,
            VideoUrl = journalEntry.VideoUrl,
            RecordedAt = journalEntry.RecordedAt,
            MotivationalMessage = journalEntry.MotivationalMessage,
            Tips = journalEntry.Tips,
            RowVersion = journalEntry.RowVersion
        };
    }

    public static JournalEntry ToDomain(this JournalEntryEntity entity)
    {
        return new JournalEntry(
            entity.Id,
            entity.PatientId,
            entity.VideoUrl,
            entity.RecordedAt,
            entity.MotivationalMessage,
            entity.Tips,
            entity.RowVersion,
            entity.AssignedPractices.Select(assignedPractice => assignedPractice.ToDomain()).ToList(),
            entity.EmotionAnalysis?.ToDomain(),
            entity.ReflectionAnswers.Select(answer => answer.ToDomain()).ToList());
    }
}
