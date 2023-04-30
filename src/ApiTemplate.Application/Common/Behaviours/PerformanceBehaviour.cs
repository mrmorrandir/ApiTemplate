﻿using System.Diagnostics;

namespace ApiTemplate.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    // private readonly ICurrentUserService _currentUserService;
    // private readonly IIdentityService _identityService;

    public PerformanceBehaviour(
        ILogger<TRequest> logger)
    // ICurrentUserService currentUserService,
    // IIdentityService identityService)
    {
        _timer = new Stopwatch();

        _logger = logger;
        // _currentUserService = currentUserService;
        // _identityService = identityService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds <= 500)
            return response;

        var requestName = typeof(TRequest).Name;
        // var userId = _currentUserService.UserId ?? string.Empty;
        // var userName = string.Empty;
        //
        // if (!string.IsNullOrEmpty(userId))
        // {
        //     userName = await _identityService.GetUserNameAsync(userId);
        // }

        // _logger.LogWarning("CleanArchitecture Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}", requestName, elapsedMilliseconds, userId, userName, request);
        _logger.LogWarning("DPS2.Processes Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", requestName, elapsedMilliseconds, request);

        return response;
    }
}