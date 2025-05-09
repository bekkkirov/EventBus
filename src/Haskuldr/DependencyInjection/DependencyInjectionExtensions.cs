using System.Reflection;
using Haskuldr.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Haskuldr.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddEventBus(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var baseHandlerType = typeof(IEventHandler<>);
        
        foreach (var assembly in assemblies)
        {
            var implementations = assembly.GetImplementations(baseHandlerType);

            foreach (var result in implementations)
            {
                services.AddScoped(result.ServiceType, result.ImplementationType);
            }
        }

        return services;
    }
}