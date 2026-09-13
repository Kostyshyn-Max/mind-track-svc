using System.ComponentModel.DataAnnotations.Schema;

namespace MaksymKostyshyn.MindTrack.Data.EF.Entities;

public class AssignedPractices
{
    public Guid Id { get; set; }
    public Guid JournalEntryId { get; set; }
    public Guid PracticeId { get; set; }
    public int PracticeStatusId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    [ForeignKey(nameof(JournalEntryId))]
    public JournalEntryEntity JournalEntry { get; set; } = null!;

    [ForeignKey(nameof(PracticeId))]
    public PracticeEntity Practice { get; set; } = null!;

    [ForeignKey(nameof(PracticeStatusId))]
    public PracticeStatusEntity PracticeStatus { get; set; } = null!;
}
