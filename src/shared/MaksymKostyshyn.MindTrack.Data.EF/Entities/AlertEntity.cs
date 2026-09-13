using System.ComponentModel.DataAnnotations.Schema;

namespace MaksymKostyshyn.MindTrack.Data.EF.Entities;

public class AlertEntity
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid TherapistId { get; set; }
    public string TriggerReason { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; }
    public bool IsResolved { get; set; }


    [ForeignKey(nameof(PatientId))]
    public UserEntity Patient { get; set; } = null!;

    [ForeignKey(nameof(TherapistId))]
    public UserEntity Therapist { get; set; } = null!;
}