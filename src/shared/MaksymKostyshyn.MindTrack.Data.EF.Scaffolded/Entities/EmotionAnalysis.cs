using System;
using System.Collections.Generic;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;

public partial class EmotionAnalysis
{
    public Guid Id { get; set; }

    public Guid JournalEntryId { get; set; }

    public string DominantEmotion { get; set; } = null!;

    public float IntonationScore { get; set; }

    public string KeyPhrasesJson { get; set; } = null!;

    public DateTime AnalyzedAt { get; set; }

    public virtual JournalEntry JournalEntry { get; set; } = null!;
}
