namespace MaksymKostyshyn.MindTrack.Domain;

public class EmotionAnalysis(
    Guid id,
    Guid journalEntryId,
    string dominantEmotion,
    float intonationScore,
    string keyPhrasesJson,
    DateTime analyzedAt)
{
    public Guid Id { get; private set; } = id;
    public Guid JournalEntryId { get; private set; } = journalEntryId;
    public string DominantEmotion { get; private set; } = dominantEmotion;
    public float IntonationScore { get; private set; } = intonationScore;
    public string KeyPhrasesJson { get; private set; } = keyPhrasesJson;
    public DateTime AnalyzedAt { get; private set; } = analyzedAt;
}
