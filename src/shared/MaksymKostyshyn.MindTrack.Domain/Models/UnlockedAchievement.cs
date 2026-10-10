namespace MaksymKostyshyn.MindTrack.Domain.Models;

public class UnlockedAchievement(
    Guid id,
    string name,
    string description,
    int pointsReward,
    DateTime unlockedAt) : Achievement(id, name, description, pointsReward)
{
    public DateTime UnlockedAt { get; private set; } = unlockedAt;
}
