using System.Reflection;

namespace Haskuldr.DependencyInjection;

public static class AssemblyExtensions
{
    public static IEnumerable<(Type ServiceType, Type ImplementationType)> GetImplementations(
        this Assembly assembly,
        Type baseType)
    {
        return assembly
               .ExportedTypes
               .Where(type => type is { IsAbstract: false, IsInterface: false })
               .Select(type => new
               {
                   ServiceType = type
                                 .GetInterfaces()
                                 .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == baseType),
                   ImplementationType = type,
               })
               .Where(x => x.ServiceType is not null)
               .Select(x => (x.ServiceType!, x.ImplementationType));
    }
}