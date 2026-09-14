namespace MaksymKostyshyn.MindTrack.Domain.Models;

public class UserAchievementReport
{
    public Guid UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public IList<Achievement> UnlockedAchievements { get; set; } = [];
}
