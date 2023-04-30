namespace ApiTemplate.Application.Common;

public interface IPagination
{
    int? Offset { get; init; }
    int? Limit { get; init; }
}