using MaksymKostyshyn.MindTrack.Domain.Enums;

namespace MaksymKostyshyn.MindTrack.Domain;

public class AssignedPractice(
    Guid id,
    Guid journalEntryId,
    Guid practiceId,
    int practiceStatusId,
    DateTime startedAt,
    DateTime? completedAt,
    Practice? practice = null)
{
    public Guid Id { get; private set; } = id;
    public Guid JournalEntryId { get; private set; } = journalEntryId;
    public Guid PracticeId { get; private set; } = practiceId;
    public PracticeStatus PracticeStatus { get; private set; } = (PracticeStatus)practiceStatusId;
    public DateTime StartedAt { get; private set; } = startedAt;
    public DateTime? CompletedAt { get; private set; } = completedAt;
    public Practice? Practice { get; private set; } = practice;
}
