using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ntt.Exceptions.ExceptionHandlers;
using Ntt.Exceptions.ExceptionTypes;

namespace Ntt.Exceptions;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        return exception switch
        {
            DatabaseException => await new DatabaseExceptionHandler(logger).TryHandleAsync(httpContext, exception,
                cancellationToken),
            UnauthorizedException => await new UnauthorizedExceptionHandler(logger).TryHandleAsync(httpContext,
                exception, cancellationToken),
            NotFoundException => await new NotFoundExceptionHandler(logger).TryHandleAsync(httpContext, exception,
                cancellationToken),
            ForbiddenException => await new ForbiddenExceptionHandler(logger).TryHandleAsync(httpContext, exception,
                cancellationToken),
            OperationalException => await new OperationalExceptionHandler(logger).TryHandleAsync(httpContext, exception,
                cancellationToken),
            BusinessException => await new BusinessExceptionHandler(logger).TryHandleAsync(httpContext, exception,
                cancellationToken),
            _ => await new SystemExceptionHandler(logger).TryHandleAsync(httpContext, exception, cancellationToken)
        };
    }
}