using System.ComponentModel.DataAnnotations.Schema;

namespace MaksymKostyshyn.MindTrack.Data.EF.Entities;

public class EmotionAnalysesEntity
{
    public Guid Id { get; set; }
    public Guid JournalEntryId { get; set; }
    public string DominantEmotion { get; set; } = string.Empty;
    public float IntonationScore { get; set; }
    public string KeyPhrasesJson { get; set; } = string.Empty;
    public DateTime AnalyzedAt { get; set; }

    [ForeignKey(nameof(JournalEntryId))]
    public JournalEntryEntity JournalEntry { get; set; } = null!;
}