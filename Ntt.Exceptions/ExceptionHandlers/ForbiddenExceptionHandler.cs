using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Ntt.Exceptions.ExceptionTypes;
using Ntt.Exceptions.Logging;

namespace Ntt.Exceptions.ExceptionHandlers;

public class ForbiddenExceptionHandler(LoggingService service) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException)
            return false;

        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        httpContext.Response.ContentType = "application/json";

        var body = exception.ToExceptionViewModel();
        var serializedBody = JsonSerializer.Serialize(body);
        
        await service.LogAsync(exception, body.Result.TraceId);

        await httpContext.Response.WriteAsync(serializedBody, cancellationToken);
        return true;
    }
}