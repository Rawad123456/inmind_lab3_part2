using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IO;

namespace lab3_inmind_part2.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private readonly RecyclableMemoryStreamManager _streamManager = new();

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var request = context.Request;
        var startTime = DateTime.UtcNow;

        
        var requestBody = await ReadRequestBody(request);

        _logger.LogInformation("HTTP Request:");
        _logger.LogInformation("️Method: {Method}", request.Method);
        _logger.LogInformation(" Path: {Path}", request.Path);
        _logger.LogInformation("Query: {Query}", request.QueryString);
        _logger.LogInformation("Headers: {Headers}", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()));
        if (!string.IsNullOrEmpty(requestBody))
            _logger.LogInformation("Body: {Body}", requestBody);
        _logger.LogInformation("Timestamp: {Time}", startTime);

        
        var originalBodyStream = context.Response.Body;
        await using var responseBody = _streamManager.GetStream();
        context.Response.Body = responseBody;

        await _next(context); 

       
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation("Response Status Code: {StatusCode}", context.Response.StatusCode);
        if (!string.IsNullOrWhiteSpace(responseText))
            _logger.LogInformation("Response Body: {Response}", responseText);

        
        await responseBody.CopyToAsync(originalBodyStream);
    }

    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering(); 

        using var stream = _streamManager.GetStream();
        await request.Body.CopyToAsync(stream);
        request.Body.Seek(0, SeekOrigin.Begin); 

        return Encoding.UTF8.GetString(stream.ToArray());
    }
}
