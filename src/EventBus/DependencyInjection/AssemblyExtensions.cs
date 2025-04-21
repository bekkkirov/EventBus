using System.Reflection;

namespace EventBus.DependencyInjection;

public static class AssemblyExtensions
{
    public static List<(Type ServiceType, Type ImplementationType)> GetImplementations(
        this Assembly assembly,
        Type baseType,
        bool onlyPublicImplementations)
    {
        var assemblyTypes = onlyPublicImplementations
            ? assembly.GetExportedTypes()
            : assembly.GetTypes();

        var implementations =
            from type in assemblyTypes
            where type is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false }
            let serviceType = type
                             .GetInterfaces()
                             .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == baseType)
            where serviceType is not null
            select (serviceType, type);

        return implementations.ToList();
    }
}