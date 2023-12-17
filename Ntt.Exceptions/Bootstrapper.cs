using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Ntt.Exceptions.Logging;
using Ntt.Exceptions.Settings;

namespace Ntt.Exceptions;

public static class Bootstrapper
{
    public static void AddNttExceptionHandler (this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CosmosDbSettings>(configuration.GetSection("CosmosSettings"));
        
        services.AddSingleton<ILogger, Logger<LoggingService>>();
        services.AddSingleton<LoggingService>();
        
        services.AddExceptionHandler<NttExceptionHandler>();
    }
}