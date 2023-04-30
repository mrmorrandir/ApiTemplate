using System.Reflection;

namespace ApiTemplate.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
     where TResponse : IResultBase
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();
        
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (!failures.Any()) return await next();

        if (typeof(TResponse).IsGenericType)
        {
            var genericType = typeof(TResponse).GetGenericArguments()[0];
            // Get the MethodInfo for the generic Result.Fail<> method
            var failMethod = typeof(Result).GetMethods(BindingFlags.Static | BindingFlags.Public).Single(m => m.Name == nameof(Result.Fail) && m.IsGenericMethod && m.GetParameters().Any(p => p.ParameterType == typeof(IEnumerable<string>)))
                ?.MakeGenericMethod(genericType);
            return (TResponse)failMethod!.Invoke(null, new object?[] {failures.Select(f => f.ErrorMessage).ToArray()})!;
        } 
        return (TResponse)typeof(TResponse).GetMethod(nameof(Result.Fail), BindingFlags.Public | BindingFlags.Static)
            ?.Invoke(null, new object?[] {failures.Select(f => f.ErrorMessage).ToArray()})!;
    }
}