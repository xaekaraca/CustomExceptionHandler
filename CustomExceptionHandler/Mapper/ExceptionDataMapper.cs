using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using NTT.Exceptions.Extractor;
using NTT.Exceptions.Model;

namespace NTT.Exceptions.Mapper;

public static class ExceptionDataMapper
{
    public static Exception AddData(this Exception exception, ExceptionModel exceptionModel, HttpContext context)
    {
        exception.Data.Add("TraceId", exceptionModel.Result.TraceId);
        exception.Data.Add("ExceptionType", exceptionModel.Result.ExceptionType);
        exception.Data.Add("Message", exceptionModel.Result.Message);
        exception.Data.Add("StatusCode", exceptionModel.StatusCode);
        exception.Data.Add("TargetSite", exception.TargetSite?.DeclaringType?.ToString());
        exception.Data.Add("Method", exception.TargetSite?.Name);
        exception.Data.Add("Payload", PayloadExtractor.GetRawPayloadAsync(context).GetAwaiter().GetResult());
        exception.Data.Add("Query", context.Request.QueryString.ToString());
        return exception;
    }
}