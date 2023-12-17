using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Ntt.Exceptions.ExceptionHandlers;
using Ntt.Exceptions.ExceptionTypes;
using Ntt.Exceptions.Logging;

namespace Ntt.Exceptions;

public class NttExceptionHandler(LoggingService loggingService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        return exception switch
        {
            DatabaseException => await new DatabaseExceptionHandler(loggingService).TryHandleAsync(httpContext, exception,
                cancellationToken),
            UnauthorizedException => await new UnauthorizedExceptionHandler(loggingService).TryHandleAsync(httpContext,
                exception, cancellationToken),
            NotFoundException => await new NotFoundExceptionHandler(loggingService).TryHandleAsync(httpContext, exception,
                cancellationToken),
            ForbiddenException => await new ForbiddenExceptionHandler(loggingService).TryHandleAsync(httpContext, exception,
                cancellationToken),
            OperationalException => await new OperationalExceptionHandler(loggingService).TryHandleAsync(httpContext, exception,
                cancellationToken),
            BusinessException => await new BusinessExceptionHandler(loggingService).TryHandleAsync(httpContext, exception,
                cancellationToken),
            _ => await new SystemExceptionHandler(loggingService).TryHandleAsync(httpContext, exception, cancellationToken)
        };
    }
}