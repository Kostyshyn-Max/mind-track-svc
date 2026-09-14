namespace MaksymKostyshyn.MindTrack.Application.Commands;

public record UpdateAchievementCommand(Guid Id, string Name, string Description, int PointsReward);
