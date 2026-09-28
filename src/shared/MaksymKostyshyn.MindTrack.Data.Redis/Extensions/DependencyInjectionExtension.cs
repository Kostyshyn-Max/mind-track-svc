using MaksymKostyshyn.MindTrack.Data.Abstractions.Caching;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Data.Redis.Queries;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace MaksymKostyshyn.MindTrack.Data.Redis.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddMindTrackRedisCache(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(connectionString));

        services.AddSingleton<ICacheService, RedisCacheService>();

        services.Decorate<IAchievementQuery>((inner, serviceProvider) =>
            new AchievementQueryCacheDecorator(inner, serviceProvider.GetRequiredService<ICacheService>()));

        return services;
    }
}
