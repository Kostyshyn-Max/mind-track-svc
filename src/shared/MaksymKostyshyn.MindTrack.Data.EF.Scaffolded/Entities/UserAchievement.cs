using System;
using System.Collections.Generic;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;

public partial class UserAchievement
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public Guid AchievementId { get; set; }

    public DateTime UnlockedAt { get; set; }

    public virtual Achievement Achievement { get; set; } = null!;

    public virtual User Patient { get; set; } = null!;
}
