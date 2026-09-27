namespace MaksymKostyshyn.MindTrack.WebSvc.Models.Achievement;

public class UpdateAchievementRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PointsReward { get; set; }
}
