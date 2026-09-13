namespace MaksymKostyshyn.MindTrack.Domain;

public class JournalEntry(
    Guid id,
    Guid patientId,
    string videoUrl,
    DateTime recordedAt,
    string motivationalMessage,
    string tips,
    byte[] rowVersion,
    IReadOnlyList<AssignedPractice>? assignedPractices = null,
    EmotionAnalysis? emotionAnalysis = null,
    IReadOnlyList<ReflectionAnswer>? reflectionAnswers = null)
{
    public Guid Id { get; private set; } = id;
    public Guid PatientId { get; private set; } = patientId;
    public string VideoUrl { get; private set; } = videoUrl;
    public DateTime RecordedAt { get; private set; } = recordedAt;
    public string MotivationalMessage { get; private set; } = motivationalMessage;
    public string Tips { get; private set; } = tips;
    public byte[] RowVersion { get; private set; } = rowVersion;
    public IReadOnlyList<AssignedPractice> AssignedPractices { get; private set; } = assignedPractices ?? [];
    public EmotionAnalysis? EmotionAnalysis { get; private set; } = emotionAnalysis;
    public IReadOnlyList<ReflectionAnswer> ReflectionAnswers { get; private set; } = reflectionAnswers ?? [];
}
