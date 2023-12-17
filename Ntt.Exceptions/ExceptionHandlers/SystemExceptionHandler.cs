using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Ntt.Exceptions.Logging;

namespace Ntt.Exceptions.ExceptionHandlers;

public class SystemExceptionHandler(LoggingService service) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var body = exception.ToExceptionViewModel();
        var serializedBody = JsonSerializer.Serialize(body);

        await service.LogAsync(exception, body.Result.TraceId);

        await httpContext.Response.WriteAsync(serializedBody, cancellationToken);
        return true;
    }
}