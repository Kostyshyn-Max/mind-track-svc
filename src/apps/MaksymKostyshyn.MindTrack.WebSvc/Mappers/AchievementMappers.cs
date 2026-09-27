using MaksymKostyshyn.MindTrack.Application.Commands;
using MaksymKostyshyn.MindTrack.WebSvc.Models.Achievement;

namespace MaksymKostyshyn.MindTrack.WebSvc.Mappers;

public static class AchievementMappers
{
    public static AddAchievementCommand ToCommand(this AddAchievementRequest request)
    {
        return new AddAchievementCommand(request.Name, request.Description, request.PointsReward);
    }

    public static UpdateAchievementCommand ToCommand(this UpdateAchievementRequest request, Guid id)
    {
        return new UpdateAchievementCommand(id, request.Name, request.Description, request.PointsReward);
    }
}
