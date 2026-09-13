namespace MaksymKostyshyn.MindTrack.Domain;

public class UserAchievement(
    Guid id,
    Guid patientId,
    Guid achievementId,
    DateTime unlockedAt)
{
    public Guid Id { get; private set; } = id;
    public Guid PatientId { get; private set; } = patientId;
    public Guid AchievementId { get; private set; } = achievementId;
    public DateTime UnlockedAt { get; private set; } = unlockedAt;
}
