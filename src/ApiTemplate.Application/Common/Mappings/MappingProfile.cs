using System.Reflection;

namespace ApiTemplate.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);
        var mapToType = typeof(IMapTo<>);
        const string mappingMethodName = nameof(IMapFrom<object>.Mapping); // "Mapping" same for IMapFrom and IMapTo   
        
        bool HasInterface(Type t) => t.IsGenericType && (t.GetGenericTypeDefinition() == mapFromType || t.GetGenericTypeDefinition() == mapToType);
        
        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();
        var argumentTypes = new Type[] { typeof(Profile) };
        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod(mappingMethodName);
            if (methodInfo is not null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {
                var interfaces = type.GetInterfaces().Where(HasInterface).ToList();
                if (interfaces.Count <= 0) continue;
                foreach (var @interface in interfaces)
                {
                    var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);
                    interfaceMethodInfo?.Invoke(instance, new object[] { this });
                }
            }
        }
    }
}