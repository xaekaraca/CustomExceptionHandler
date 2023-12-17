using Microsoft.Extensions.Logging;
using Ntt.Exceptions.Model;

namespace Ntt.Exceptions.Mapper;

public static class LogMapper
{
    public static LogModel ToLogModel(Exception exception, Guid traceId)
    {
        return new LogModel
        {
            Id = traceId.ToString(),
            Level = LogLevel.Error,
            Message = exception.Message,
            Source = exception.Source,
            TargetSite = exception.TargetSite?.Name,
            StackTrace = exception.StackTrace,
            CreatedAt = DateTime.UtcNow
        };
    }
}