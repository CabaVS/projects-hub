using System.Diagnostics;
using CabaVS.ExpenseTracker.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CabaVS.ExpenseTracker.Application.Common.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        
        logger.LogInformation("Handling {RequestName} with payload: {@Request}.", requestName, request);
        
        var stopwatch = Stopwatch.StartNew();
        TResponse response = await next(cancellationToken);
        stopwatch.Stop();
        
        switch (response)
        {
            case Result { IsSuccess: true }:
                logger.LogInformation(
                    "Handled {RequestName} in {ElapsedMilliseconds} ms.", 
                    requestName, 
                    stopwatch.ElapsedMilliseconds);
                break;
            case Result { IsFailure: true } result:
                logger.LogWarning(
                    "Failed {RequestName} with {Error} in {ElapsedMilliseconds} ms.",
                    requestName,
                    result.Error,
                    stopwatch.ElapsedMilliseconds);
                break;
            default:
                logger.LogInformation(
                    "Handled {RequestName} in {ElapsedMilliseconds} ms.",
                    requestName,
                    stopwatch.ElapsedMilliseconds);
                break;
        }
        
        return response;
    }
}
