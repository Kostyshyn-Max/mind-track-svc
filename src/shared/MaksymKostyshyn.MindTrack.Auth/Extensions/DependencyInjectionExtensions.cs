using System.Text;
using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Auth.Authorization;
using MaksymKostyshyn.MindTrack.Auth.Options;
using MaksymKostyshyn.MindTrack.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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

        services.AddMindTrackJwtAuthentication(configuration);

        return services;
    }

    private static IServiceCollection AddMindTrackJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new InvalidOperationException($"Configuration section '{JwtOptions.SectionName}' is missing.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

        services.AddAuthorization();
        services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();

        return services;
    }
}
