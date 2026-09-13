namespace MaksymKostyshyn.MindTrack.Domain;

public class ReflectionAnswer(
    Guid id,
    Guid journalEntryId,
    string questions,
    string answers,
    Guid? previousAnswerId,
    DateTime answeredAt)
{
    public Guid Id { get; private set; } = id;
    public Guid JournalEntryId { get; private set; } = journalEntryId;
    public string Questions { get; private set; } = questions;
    public string Answers { get; private set; } = answers;
    public Guid? PreviousAnswerId { get; private set; } = previousAnswerId;
    public DateTime AnsweredAt { get; private set; } = answeredAt;
}
