using System;
using System.Collections.Generic;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;

public partial class Alert
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public Guid TherapistId { get; set; }

    public string TriggerReason { get; set; } = null!;

    public string Severity { get; set; } = null!;

    public DateTime DetectedAt { get; set; }

    public bool IsResolved { get; set; }

    public virtual User Patient { get; set; } = null!;

    public virtual User Therapist { get; set; } = null!;
}
