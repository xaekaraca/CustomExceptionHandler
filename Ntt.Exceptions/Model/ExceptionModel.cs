namespace Ntt.Exceptions;

public class ExceptionModel
{
    public int StatusCode { get; set; }
    
    public ExceptionDetailModel Result { get; init; } = new();
}

public class ExceptionDetailModel
{
    public Guid TraceId { get; init; }

    public string ExceptionType { get; set; } = null!;
    public string? Message { get; set; }
    
    
}

