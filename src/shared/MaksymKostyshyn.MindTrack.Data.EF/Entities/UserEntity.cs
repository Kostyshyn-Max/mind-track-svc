using System.ComponentModel.DataAnnotations.Schema;

namespace MaksymKostyshyn.MindTrack.Data.EF.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public Guid? TherapistId { get; set; }
    public bool IsVulnerable { get; set; }
    public int AIResponseStyleId { get; set; }
    public int EarnedPoints { get; set; } = 0;

    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ForeignKey(nameof(RoleId))]
    public UserRoleEntity Role { get; set; } = null!;

    [ForeignKey(nameof(AIResponseStyleId))]
    public AIResponseStyleEntity AIResponseStyle { get; set; } = null!;

    [ForeignKey(nameof(TherapistId))]
    public UserEntity? Therapist { get; set; }

    public IList<JournalEntryEntity> JournalEntries { get; set; } = [];
}
