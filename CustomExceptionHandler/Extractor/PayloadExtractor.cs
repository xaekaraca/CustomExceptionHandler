using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace NTT.Exceptions.Extractor;

public static class PayloadExtractor
{
    public static async Task<JObject?> GetRawPayloadAsync(HttpContext httpContext)
    {
        var contentType = httpContext.Request.ContentType;

        if (contentType == null)
            return null;

        try
        {
            if (contentType.Contains("application/json"))
            {
                if (httpContext.Request.Body.CanSeek)
                {
                    httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                }

                using var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);
                var jsonPayload = await reader.ReadToEndAsync();
                return JObject.Parse(jsonPayload);
            }

            if (contentType.Contains("multipart/form-data") || contentType.Contains("application/x-www-form-urlencoded"))
            {
                var formCollection = await httpContext.Request.ReadFormAsync();
                var jsonObject = new JObject();

                foreach (var field in formCollection)
                {
                    jsonObject.Add(field.Key, JToken.FromObject(field.Value.ToString()));
                }

                foreach (var file in httpContext.Request.Form.Files)
                {
                    jsonObject.Add(file.Name, new JObject
                    {
                        { "FileName", file.FileName },
                        { "ContentType", file.ContentType },
                    });
                }

                return jsonObject;
            }
        }
        catch (Exception)
        {
            return new JObject
            {
                { "Error", "Failed to parse payload." }
            };
        }
        
        return null;
    }
}