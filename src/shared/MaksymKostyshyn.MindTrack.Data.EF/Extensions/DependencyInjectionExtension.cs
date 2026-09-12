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

        return services;
    }
}
