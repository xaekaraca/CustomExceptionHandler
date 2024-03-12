using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NTT.Exceptions.Mapper;

namespace NTT.Exceptions.ExceptionHandlers;

public class SystemExceptionHandler(ILogger logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var body = exception.ToExceptionViewModel();
        var serializedBody = JsonSerializer.Serialize(body);

        exception = exception.AddData(body, httpContext);
        
        logger.LogCritical(
            exception,
            "SystemException occurred with the traceId: {TraceId}",
            body.Result.TraceId);
        
        await httpContext.Response.WriteAsync(serializedBody, cancellationToken);
        return true;
    }
}