using Microsoft.Extensions.Logging;

namespace App.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogError(ex, "BlissLobby Request: Unhandled Exception of type {ExceptionType} for Request {Name} {@Request}", ex.GetType().Name, requestName, request);

            throw;
        }
    }
}
