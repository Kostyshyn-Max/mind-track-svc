using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Data.ADO.Queries;
using MaksymKostyshyn.MindTrack.Data.ADO.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MaksymKostyshyn.MindTrack.Data.ADO.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddMindTrackAdoDataAccess(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(new SqlConnectionFactory(connectionString));

        services.AddScoped<IAchievementRepository, AchievementRepository>();
        services.AddScoped<IAlertRepository, AlertRepository>();

        services.AddScoped<IAchievementQuery, AchievementQuery>();

        return services;
    }
}
