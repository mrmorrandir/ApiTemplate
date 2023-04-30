using System.Reflection;

namespace ApiTemplate.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResultBase
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            if (typeof(TResponse).IsGenericType)
            {
                var genericType = typeof(TResponse).GetGenericArguments()[0];
                // Get the MethodInfo for the generic Result.Fail<> method
                var failMethod = typeof(Result)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Single(m => m.Name == nameof(Result.Fail) && m.IsGenericMethod && m.GetParameters().Any(p => p.ParameterType == typeof(IEnumerable<string>)))
                    .MakeGenericMethod(genericType);
                return (TResponse)failMethod.Invoke(null, new[] { new []{ ex.Message } });
            }
            // Reflection is slow, so we cache the MethodInfo for the static Result.Fail(string) method
            
            
            return (TResponse)typeof(TResponse)
                .GetMethod(nameof(Result.Fail), 0, new []{ typeof(string) })?
                .Invoke(null, new[] { ex.Message });
        }
    }
}