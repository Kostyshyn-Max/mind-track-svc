using System;
using System.Collections.Generic;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int RoleId { get; set; }

    public Guid? TherapistId { get; set; }

    public bool IsVulnerable { get; set; }

    public int AiresponseStyleId { get; set; }

    public int EarnedPoints { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual AiresponseStyle AiresponseStyle { get; set; } = null!;

    public virtual ICollection<Alert> AlertPatients { get; set; } = new List<Alert>();

    public virtual ICollection<Alert> AlertTherapists { get; set; } = new List<Alert>();

    public virtual ICollection<User> InverseTherapist { get; set; } = new List<User>();

    public virtual ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();

    public virtual UserRole Role { get; set; } = null!;

    public virtual User? Therapist { get; set; }

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
