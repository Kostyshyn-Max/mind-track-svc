using System.ComponentModel.DataAnnotations.Schema;

namespace MaksymKostyshyn.MindTrack.Data.EF.Entities;

public class UserAchievementEntity
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid AchievementId { get; set; }
    public DateTime UnlockedAt { get; set; }

    [ForeignKey(nameof(PatientId))]
    public UserEntity Patient { get; set; } = null!;

    [ForeignKey(nameof(AchievementId))]
    public AchievementEntity Achievement { get; set; } = null!;
}
