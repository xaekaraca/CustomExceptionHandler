using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ntt.Exceptions.Model;

public class LogModel
{
    [Key]
    [JsonProperty("id")]
    public string Id { get; init; } = null!;

    public LogLevel Level { get; init; }
    
    public string? Message { get; init; }
    
    public string? Source { get; init; }
    
    public string? TargetSite { get; init; }
    
    public string? StackTrace { get; init; }
    
    public DateTime CreatedAt { get; init; }
}