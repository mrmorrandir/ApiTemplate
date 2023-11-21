namespace ApiTemplate.Application.Common;

public interface IFilterRange<T> where T : struct
{
    T? Min { get; set; }
    T? Max { get; set; }
}