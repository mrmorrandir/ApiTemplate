using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.API.Common.Extensions;


public static class MediatorExtensions
{
    public static async Task<ActionResult<T>> SendAndReturnObjectActionResult<T>(this IMediator mediator, IRequest<Result<T>> request)
    {
        var result = await mediator.Send(request);
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);
        return new BadRequestObjectResult(ToEnumerable(result));
    }

    public static async Task<ActionResult> SendAndReturnActionResult(this IMediator mediator, IRequest<Result> request)
    {
        var result = await mediator.Send(request);
        if (result.IsSuccess)
            return new OkResult();
        return new BadRequestObjectResult(ToEnumerable(result));
    }
    
    public static ActionResult<T> ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);
        return new BadRequestObjectResult(ToEnumerable(result));
    }
    
    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkResult();
        return new BadRequestObjectResult(ToEnumerable(result));
    }

    private static IEnumerable<string> ToEnumerable(ResultBase result, string indent = "") 
    {
        var output = new List<string>();       
        output.AddRange(ToEnumerable(result.Errors, indent));
        return output;
    }

    private static IEnumerable<string> ToEnumerable(this IEnumerable<IError> errors, string indent = "") 
    {
        var output = new List<string>();
        foreach (var error in errors)
            output.AddRange(ToEnumerable(error, indent));
        return output;
    }

    private static IEnumerable<string> ToEnumerable(this IError error, string indent = "") 
    {
        var output = new List<string>();
        output.Add($"{indent}{error.Message}");
        if (!error.Reasons.Any()) return output;
        
        foreach (var reason in error.Reasons)
            output.AddRange(ToEnumerable(reason, indent));

        return output;
    }
    
    public static string ToOutputString(this IError error, string indent = "") 
    {
        var output = new StringBuilder();
        output.AppendLine($"{indent}{error.Message}");
        if (error.Reasons.Any())
        {
            foreach (var reason in error.Reasons)
            {
                var reasonOutput = ToOutputString(reason, $"  {indent}");
                output.AppendLine(reasonOutput);
            }
        }
        return output.ToString().TrimEnd();
    }
}