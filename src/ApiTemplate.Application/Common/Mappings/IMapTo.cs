namespace ApiTemplate.Application.Common.Mappings;

public interface IMapTo<T>
{
    void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T)).ForAllMembers(m => m.Condition((source, target, sourceValue, targetValue) => sourceValue != null));
}