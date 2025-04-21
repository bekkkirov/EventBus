using System.Reflection;
using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddEventBus(
        this IServiceCollection services,
        List<Assembly> assemblies,
        bool onlyPublicHandlers = true)
    {
        var baseHandlerType = typeof(IEventHandler<>);
        
        foreach (var assembly in assemblies)
        {
            var implementations = assembly.GetImplementations(
                baseHandlerType,
                onlyPublicHandlers);

            foreach (var result in implementations)
            {
                services.AddScoped(result.ServiceType, result.ImplementationType);
            }
        }

        return services;
    }
}