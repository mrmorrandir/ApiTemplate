using System.Globalization;
using FluentResults;
using MediatR;

namespace ApiTemplate.Application.Common;

public interface IFilterRange<T> where T : struct
{
    T? Min { get; set; }
    T? Max { get; set; }
}

public interface ICultureRequest<T> : IRequest<Result<T>>
{
    /// <summary>
    /// Culture Info for Request
    /// </summary>
    CultureInfo CultureInfo { get; set; }
}

public interface ICultureRequest : IRequest<Result>
{
    /// <summary>
    /// Culture Info for Request
    /// </summary>
    CultureInfo CultureInfo { get; set; }
}