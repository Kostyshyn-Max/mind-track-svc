using FluentValidation;
using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Application.Commands;
using MaksymKostyshyn.MindTrack.Application.Services;
using MaksymKostyshyn.MindTrack.Application.UseCases.Achievements;
using MaksymKostyshyn.MindTrack.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace MaksymKostyshyn.MindTrack.Application.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddMindTrackApplication(this IServiceCollection services)
    {
        services.AddScoped<IAchievementService, AchievementService>();

        services.AddScoped<IValidator<AddAchievementCommand>, AddAchievementCommandValidator>();
        services.AddScoped<IValidator<UpdateAchievementCommand>, UpdateAchievementCommandValidator>();

        services.AddScoped<AddAchievementUseCase>();
        services.AddScoped<GetAchievementUseCase>();
        services.AddScoped<UpdateAchievementUseCase>();
        services.AddScoped<DeleteAchievementUseCase>();

        return services;
    }
}
