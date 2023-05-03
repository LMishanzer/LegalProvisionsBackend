using System.Diagnostics;
using System.Text;
using LegalProvisionsLib.Logging.Models;
using ILogger = LegalProvisionsLib.Logging.ILogger;

namespace LegalProvisionsBackend.Middleware;

public class RequestMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public RequestMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Path.Value?.Contains("file") ?? false)
        {
            await _next(httpContext);

            return;
        }
        
        var userIp = httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

        var url = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path.Value}";
            
        httpContext.Request.EnableBuffering();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            await _next(httpContext);
        }
        catch (Exception)
        {
            httpContext.Response.StatusCode = 500;

            throw;
        }
        finally
        {
            try
            {
                stopwatch.Stop();
                var millisecondsElapsed = stopwatch.ElapsedMilliseconds;

                httpContext.Request.Body.Position = 0;
                using var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);
                var bodyString = await reader.ReadToEndAsync();
                httpContext.Request.Body.Position = 0;

                var requestLog = new RequestLog
                {
                    Ip = userIp ?? string.Empty,
                    Url = url,
                    Body = bodyString,
                    RequestDuration = millisecondsElapsed,
                    Method = httpContext.Request.Method,
                    StatusCode = httpContext.Response.StatusCode
                };

                _logger.LogRequest(requestLog);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }
    }
}