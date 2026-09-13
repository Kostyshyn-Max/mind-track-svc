using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.EF.Mappers;

public static class AlertMapper
{
    public static AlertEntity ToEntity(this Alert alert)
    {
        return new AlertEntity
        {
            Id = alert.Id,
            PatientId = alert.PatientId,
            TherapistId = alert.TherapistId,
            TriggerReason = alert.TriggerReason,
            Severity = alert.Severity,
            DetectedAt = alert.DetectedAt,
            IsResolved = alert.IsResolved
        };
    }

    public static Alert ToDomain(this AlertEntity entity)
    {
        return new Alert(
            entity.Id,
            entity.PatientId,
            entity.TherapistId,
            entity.TriggerReason,
            entity.Severity,
            entity.DetectedAt,
            entity.IsResolved);
    }
}
