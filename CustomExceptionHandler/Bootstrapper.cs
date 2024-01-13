using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace Ntt.Exceptions;

public static class Bootstrapper
{
    public static void AddCustomExceptionHandler (this IServiceCollection services)
    {
        services.AddLogging();
        
        services.AddExceptionHandler<CustomExceptionHandler>();
    }
    
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var originalBodyStream = context.Response.Body;

            // Clone the request body stream
            using var memoryStream = new MemoryStream();
            await context.Request.Body.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = memoryStream;

            // Call the next middleware
            await next();

            // Reset the body stream after the request handling
            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        });
        
        app.UseExceptionHandler(_ => { });
    }
}