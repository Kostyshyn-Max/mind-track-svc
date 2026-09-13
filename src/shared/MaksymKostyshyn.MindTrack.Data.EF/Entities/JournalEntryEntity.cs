using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaksymKostyshyn.MindTrack.Data.EF.Entities;

public class JournalEntryEntity
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string VideoUrl { get; set; } = string.Empty;
    public DateTime RecordedAt { get; set; }
    public string MotivationalMessage { get; set; } = string.Empty;
    public string Tips { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];

    [ForeignKey(nameof(PatientId))]
    public UserEntity Patient { get; set; } = null!;

    public IList<AssignedPractices> AssignedPractices { get; set; } = [];

    public EmotionAnalysesEntity? EmotionAnalysis { get; set; }

    public IList<ReflectionAnswerEntity> ReflectionAnswers { get; set; } = [];
}