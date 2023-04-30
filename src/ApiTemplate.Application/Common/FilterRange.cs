namespace ApiTemplate.Application.Common;

public class FilterRange<T> : IFilterRange<T> where T : struct
{
    public T? Min { get; set; }
    public T? Max { get; set; }
}