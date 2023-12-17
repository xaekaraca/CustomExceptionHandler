namespace Ntt.Exceptions.Settings;

public class CosmosDbSettings
{
    public string? ConnectionString { get; set; }

    public const string ContainerName = "Logs";

    public string? DatabaseName { get; set; }
}