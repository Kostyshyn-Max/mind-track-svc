namespace MaksymKostyshyn.MindTrack.WebSvc.Models;

public class AddAchievementRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PointsReward { get; set; }
}
