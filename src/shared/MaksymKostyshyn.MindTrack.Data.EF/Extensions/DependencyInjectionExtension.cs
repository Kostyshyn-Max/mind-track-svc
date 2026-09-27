using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Data.EF.Queries;
using MaksymKostyshyn.MindTrack.Data.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MaksymKostyshyn.MindTrack.Data.EF.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddMindTrackEntityFrameworkDataAccess(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<MindTrackDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserQuery, UserQuery>();

        return services;
    }
}
