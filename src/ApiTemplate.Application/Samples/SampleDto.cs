using ApiTemplate.Domain;

namespace ApiTemplate.Application.Samples;

public class SampleDto : IMapFrom<Sample>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}