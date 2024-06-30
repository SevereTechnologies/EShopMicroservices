using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse> // generic behavior
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger) // inject Logger
    : IPipelineBehavior<TRequest, TResponse> // inherit from IPipelineBehavior
    where TRequest : notnull, IRequest<TResponse> // IRequest<TResponse> mean inherits from TResponse object
    where TResponse : notnull
{
    /// <summary>
    /// Handle and return a task of type tresponse.
    /// </summary>
    /// <param name="request">The request pass to the handler.</param>
    /// <param name="next">The delegate next call in the pipeline to handle the next operation.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><![CDATA[Task<TResponse>]]></returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeTaken = timer.Elapsed;

        if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then log the warnings
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
                typeof(TRequest).Name, timeTaken.Seconds);

        logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);
        return response;
    }
}