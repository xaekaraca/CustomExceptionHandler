using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ntt.Exceptions.ExceptionTypes;
using Ntt.Exceptions.Mapper;

namespace Ntt.Exceptions.ExceptionHandlers;

public class ForbiddenExceptionHandler(ILogger logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException)
            return false;

        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        
        httpContext.Response.ContentType = "application/json";

        var body = exception.ToExceptionViewModel();
        var serializedBody = JsonSerializer.Serialize(body);
        
        exception = exception.AddData(body, httpContext);

        logger.LogWarning(exception,
            "ForbiddenException occured with the traceId: {TraceId}",
            body.Result.TraceId);
            
        
        await httpContext.Response.WriteAsync(serializedBody, cancellationToken);
        return true;
    }
}