using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Auth.Options;
using MaksymKostyshyn.MindTrack.Auth.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaksymKostyshyn.MindTrack.Auth.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMindTrackAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<GoogleOAuthOptions>()
            .Bind(configuration.GetSection(GoogleOAuthOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpClient(nameof(GoogleOAuthService));

        services.AddScoped<IGoogleOAuthService, GoogleOAuthService>();
        services.AddScoped<IAppTokenGeneratorService, JwtTokenGeneratorService>();

        return services;
    }
}
