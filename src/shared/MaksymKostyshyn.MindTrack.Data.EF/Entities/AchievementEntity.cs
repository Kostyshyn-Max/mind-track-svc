namespace MaksymKostyshyn.MindTrack.Data.EF.Entities;

public class AchievementEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set;  } = string.Empty;
    public int PointsReward { get; set; }
}
