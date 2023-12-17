This is a testing package for Customized Exception Handling.

Requires .NET 8.0 or higher.

## Prerequisites

CosmosSettings on your AppSettings.json or AppSettings.Development.json

```json
{
  "CosmosSettings": {
    "ConnectionString": "YOUR_CONNECTION_STRING",
    "DatabaseName": "YOUR_DATABASE_NAME"
  }
}
```

## Usage

```csharp 
builder.Services.AddNttExceptionHandler(builder.Configuration);
```

```csharp
app.UseNttExceptionHandler(_ => { } );
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
