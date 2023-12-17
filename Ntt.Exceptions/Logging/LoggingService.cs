using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ntt.Exceptions.Mapper;
using Ntt.Exceptions.Model;
using Ntt.Exceptions.Settings;

namespace Ntt.Exceptions.Logging;

public class LoggingService
{
    private readonly CosmosClient? _cosmosClient;
    private Container? _container;
    private readonly ILogger _logger;

    public LoggingService(IOptions<CosmosDbSettings> settings, ILogger logger)
    {
        _logger = logger;
        
        if (settings.Value.ConnectionString is null || settings.Value.DatabaseName is null) 
            return;
        
        _cosmosClient = new CosmosClient(settings.Value.ConnectionString);
        CreateDatabaseAndContainerIfNotExistsAsync(settings.Value.DatabaseName, CosmosDbSettings.ContainerName).Wait();
    }

    private async Task CreateDatabaseAndContainerIfNotExistsAsync(string databaseName, string containerName)
    {
        var database = await _cosmosClient!.CreateDatabaseIfNotExistsAsync(databaseName);
        var cosmosContainer = await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
        _container = cosmosContainer.Container;
    }

    private async Task InsertLogToCosmosAsync(LogModel logEntry)
    {
        await _container!.CreateItemAsync(logEntry, new PartitionKey(logEntry.Id));
    }
    
    private void AddLogAsync(LogModel logEntry)
    { 
        _logger.Log(logEntry.Level, message: logEntry.Message, logEntry.Id, logEntry.Source, logEntry.TargetSite,
            logEntry.StackTrace, logEntry.CreatedAt);
    }

    public Task LogAsync(Exception exception, Guid traceId)
    {
        switch (_container is null)
        {
            case true:
                AddLogAsync(LogMapper.ToLogModel(exception, traceId));
                break;
            default:
                return InsertLogToCosmosAsync(LogMapper.ToLogModel(exception, traceId));
        }

        return Task.CompletedTask;
    }
}
