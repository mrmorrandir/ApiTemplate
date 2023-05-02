using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.API.Common.Extensions;

public static class MediatorExtensions
{
    public static async Task<ActionResult<T>> SendAndReturnObjectActionResult<T>(this IMediator mediator, IRequest<Result<T>> request)
    {
        var result = await mediator.Send(request);
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);
        return new BadRequestObjectResult(result.Errors.Select(e => e.Message).ToList());
    }

    public static async Task<ActionResult> SendAndReturnActionResult(this IMediator mediator, IRequest<Result> request)
    {
        var result = await mediator.Send(request);
        if (result.IsSuccess)
            return new OkResult();
        return new BadRequestObjectResult(result.Errors.Select(e => e.Message).ToList());
    }
}