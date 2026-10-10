using FluentValidation;
using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Application.Commands;
using MaksymKostyshyn.MindTrack.Application.Services;
using MaksymKostyshyn.MindTrack.Application.UseCases.Achievements;
using MaksymKostyshyn.MindTrack.Application.UseCases.Auth;
using MaksymKostyshyn.MindTrack.Application.UseCases.Users;
using MaksymKostyshyn.MindTrack.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace MaksymKostyshyn.MindTrack.Application.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddMindTrackApplication(this IServiceCollection services)
    {
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IExchangeTokenService, ExchangeTokenService>();

        services.RegisterValidators();
        services.RegisterUseCases();

        return services;
    }

    private static void RegisterValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AddAchievementCommand>, AddAchievementCommandValidator>();
        services.AddScoped<IValidator<UpdateAchievementCommand>, UpdateAchievementCommandValidator>();
        services.AddScoped<IValidator<AddUserCommand>, AddUserCommandValidator>();
        services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
        services.AddScoped<IValidator<ExchangeTokenCommand>,  ExchangeTokenCommandValidator>();
    }

    private static void RegisterUseCases(this IServiceCollection services)
    {
        services.AddScoped<AddAchievementUseCase>();
        services.AddScoped<GetAchievementUseCase>();
        services.AddScoped<GetAllAchievementsUseCase>();
        services.AddScoped<GetUnlockedAchievementsUseCase>();
        services.AddScoped<UpdateAchievementUseCase>();
        services.AddScoped<DeleteAchievementUseCase>();

        services.AddScoped<AddUserUseCase>();
        services.AddScoped<GetUserUseCase>();
        services.AddScoped<UpdateUserUseCase>();
        services.AddScoped<DeleteUserUseCase>();

        services.AddScoped<ExchangeTokenUseCase>();
    }
}
