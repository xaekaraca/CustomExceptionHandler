This is a testing package for Customized Exception Handling.

Requires .NET 8.0 or higher.

## Usage

```csharp 
builder.Services.AddNTTExceptionHandler();
```

```csharp
app.UseNTTExceptionHandler();
```

```csharp
throw new BusinessException("This is a test business exception.");
```

### Result
```json 
{
  "StatusCode": 400,
  "Result": {
    "TraceId": "20fd343d-b08e-4545-8547-ee994fda9e4d",
    "ExceptionType": "Business.Exception",
    "Message": "This is a test business exception."
  }
}
```

## Notes for Serilog

```csharp

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "YOURAPPLICATIONNAME")
    .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariables())
    .Filter.ByIncludingOnly( e => e.Exception is not null)
    .Filter.ByExcluding(e=>e.Properties.ContainsKey("EventId"))
    .WriteTo.AzureCosmosDB(endpointUri: new Uri("cosmos-uri"),
        "auth-key"
    )
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
```

    .Filter.ByExcluding(e=>e.Properties.ContainsKey("EventId"))

If you do not add this filter, Microsoft Diagnostics will log a duplicated exception.

    .Filter.ByIncludingOnly( e => e.Exception is not null)

If you do not add this filter, Serilog will be logging everything about the application.

Make sure to make some research about Serilog before using it.

## Notes for Form-Data

If you want to log form-data payload. You should handle anti-forgery token.

Either disable for the particular action or add the token to the form-data.


## Incoming Features

- [ ] AI integration for translating exception messages.



 