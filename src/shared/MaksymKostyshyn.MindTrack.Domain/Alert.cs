namespace MaksymKostyshyn.MindTrack.Domain;

public class Alert(
    Guid id,
    Guid patientId,
    Guid therapistId,
    string triggerReason,
    string severity,
    DateTime detectedAt,
    bool isResolved)
{
    public Guid Id { get; private set; } = id;
    public Guid PatientId { get; private set; } = patientId;
    public Guid TherapistId { get; private set; } = therapistId;
    public string TriggerReason { get; private set; } = triggerReason;
    public string Severity { get; private set; } = severity;
    public DateTime DetectedAt { get; private set; } = detectedAt;
    public bool IsResolved { get; private set; } = isResolved;
}
