using System.ComponentModel.DataAnnotations.Schema;

namespace MaksymKostyshyn.MindTrack.Data.EF.Entities;

public class ReflectionAnswerEntity
{
    public Guid Id { get; set; }
    public Guid JournalEntryId { get; set; }
    public string Questions { get; set; } = string.Empty;
    public string Answers { get; set; } = string.Empty;
    public Guid? PreviousAnswerId { get; set; }
    public DateTime AnsweredAt { get; set; }

    [ForeignKey(nameof(PreviousAnswerId))]
    public ReflectionAnswerEntity? ReflectionAnswer { get; set; }
}
