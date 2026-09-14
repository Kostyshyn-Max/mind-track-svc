using System;
using System.Collections.Generic;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;

public partial class JournalEntry
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public string VideoUrl { get; set; } = null!;

    public DateTime RecordedAt { get; set; }

    public string MotivationalMessage { get; set; } = null!;

    public string Tips { get; set; } = null!;

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<AssignedPractice> AssignedPractices { get; set; } = new List<AssignedPractice>();

    public virtual EmotionAnalysis? EmotionAnalysis { get; set; }

    public virtual User Patient { get; set; } = null!;

    public virtual ICollection<ReflectionAnswer> ReflectionAnswers { get; set; } = new List<ReflectionAnswer>();
}
