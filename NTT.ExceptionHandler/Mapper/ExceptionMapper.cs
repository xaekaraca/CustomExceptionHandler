using NTT.Exceptions.ExceptionTypes;
using NTT.Exceptions.Model;

namespace NTT.Exceptions.Mapper;

public static class ExceptionMapper
{
    public static ExceptionModel ToExceptionViewModel(this Exception exception)
    {
        return exception switch
        {
            BusinessException businessException => new ExceptionModel
            {
                StatusCode = 400,
                Result = new ExceptionDetailModel
                {
                    TraceId = Guid.NewGuid(),
                    ExceptionType = ExceptionConstants.BusinessExceptionName,
                    Message = businessException.Message
                }
            },
            UnauthorizedException unauthorizedException => new ExceptionModel
            {
                StatusCode = 401,
                Result = new ExceptionDetailModel
                {
                    TraceId = Guid.NewGuid(),
                    ExceptionType = ExceptionConstants.UnauthorizedExceptionName,
                    Message = unauthorizedException.Message
                }
            },
            ForbiddenException forbiddenException => new ExceptionModel
            {
                StatusCode = 403,
                Result = new ExceptionDetailModel
                {
                    TraceId = Guid.NewGuid(),
                    ExceptionType = ExceptionConstants.ForbiddenExceptionName,
                    Message = forbiddenException.Message
                }
            },
            NotFoundException notFoundException => new ExceptionModel
            {
                StatusCode = 404,
                Result = new ExceptionDetailModel
                {
                    TraceId = Guid.NewGuid(),
                    ExceptionType = ExceptionConstants.NotFoundExceptionName,
                    Message = notFoundException.Message
                }
            },
            DatabaseException databaseException => new ExceptionModel
            {
                StatusCode = 500,
                Result = new ExceptionDetailModel
                {
                    TraceId = Guid.NewGuid(),
                    ExceptionType = ExceptionConstants.DatabaseExceptionName,
                    Message = databaseException.Message
                }
            },
            OperationalException operationalException => new ExceptionModel
            {
                StatusCode = 500,
                Result = new ExceptionDetailModel
                {
                    TraceId = Guid.NewGuid(),
                    ExceptionType = ExceptionConstants.OperationalExceptionName,
                    Message = operationalException.Message
                }
            },
            _ => new ExceptionModel
            {
                StatusCode = 500,
                Result = new ExceptionDetailModel
                {
                    TraceId = Guid.NewGuid(),
                    ExceptionType = ExceptionConstants.SystemExceptionName,
                    Message = exception.Message
                }
            }
        };
    }
}