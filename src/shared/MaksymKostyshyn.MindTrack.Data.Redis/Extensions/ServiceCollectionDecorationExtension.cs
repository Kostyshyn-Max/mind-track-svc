using Microsoft.Extensions.DependencyInjection;

namespace MaksymKostyshyn.MindTrack.Data.Redis.Extensions;

internal static class ServiceCollectionDecorationExtension
{
    public static IServiceCollection Decorate<TService>(
        this IServiceCollection services,
        Func<TService, IServiceProvider, TService> decorate)
        where TService : notnull
    {
        var descriptor = services.LastOrDefault(d => d.ServiceType == typeof(TService))
            ?? throw new InvalidOperationException($"Service '{typeof(TService).Name}' must be registered before it can be decorated.");

        var index = services.IndexOf(descriptor);

        object CreateInner(IServiceProvider serviceProvider)
        {
            if (descriptor.ImplementationInstance is not null)
            {
                return descriptor.ImplementationInstance;
            }

            if (descriptor.ImplementationFactory is not null)
            {
                return descriptor.ImplementationFactory(serviceProvider);
            }

            return ActivatorUtilities.CreateInstance(serviceProvider, descriptor.ImplementationType!);
        }

        services[index] = ServiceDescriptor.Describe(
            typeof(TService),
            serviceProvider => decorate((TService)CreateInner(serviceProvider), serviceProvider)!,
            descriptor.Lifetime);

        return services;
    }
}
