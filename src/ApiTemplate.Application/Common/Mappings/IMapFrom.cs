namespace ApiTemplate.Application.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType()).ForAllMembers(m => m.Condition((source, target, sourceValue, targetValue) => sourceValue != null));
}